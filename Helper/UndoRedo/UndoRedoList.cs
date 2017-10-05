using System;
using System.Collections.Generic;

namespace Helper.UndoRedo
{
    public class UndoRedoList<T> : List<T>, IUndoRedoList<T>
    {
        private delegate void ListMethod();

        private List<State> History
        {
            get;
            set;
        }

        private int HistoryIndex
        {
            get;
            set;
        }

        public bool CanUndo => HistoryIndex < History.Count; 
        public bool CanRedo => HistoryIndex > 0;

        public UndoRedoList() : base()
        {
            History = new List<State>();
        }
        public UndoRedoList(int capacity) : base(capacity)
        {
            History = new List<State>();
        }
        public UndoRedoList(IEnumerable<T> collection) : base(collection)
        {
            History = new List<State>();
        }

        public new T this[int index]
        {
            get => base[index];
            set
            {
                var oldValue = base[index];
                ListMethod next = () => base[index] = value;
                ListMethod last = () => base[index] = oldValue;

                AddHistoryData(last, next);
                next();
            }
        }

        public List<int> FindAllIndexes(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var indexes = new List<int>();
            for (int i = 0; i < Count; i++)
                if (match(this[i]))
                    indexes.Add(i);
            return indexes;
        }

        private List<(int index, T item)> FindAllIndexMatches(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var indexes = new List<(int index, T item)>();
            for (int i = 0; i < Count; i++)
                if (match(this[i]))
                    indexes.Add((i, this[i]));
            return indexes;
        }

        public new void Add(T item)
        {
            int index = Count;
            ListMethod next = () => base.Add(item);
            ListMethod last = () => base.RemoveAt(index);

            AddHistoryData(last, next);
            next();
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            int index = Count;
            ListMethod next = () => base.AddRange(collection);
            next();
            int count = Count;
            ListMethod last = () => base.RemoveRange(index, count - index);
            AddHistoryData(last, next);
        }

        public new void Clear()
        {
            var collection = new List<T>(this);
            ListMethod next = () => base.Clear();
            ListMethod last = () => base.AddRange(collection);

            AddHistoryData(() => base.AddRange(collection), () => base.Clear());
            next();
        }

        public new void Insert(int index, T item)
        {
            ListMethod next = () => base.Insert(index, item);
            ListMethod last = () => base.RemoveAt(index);

            next();
            AddHistoryData(next, last);
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            int start = Count;
            ListMethod next = () => base.InsertRange(index, collection);
            next();
            int count = Count;
            ListMethod last = () => base.RemoveRange(index, count - index);
            AddHistoryData(last, next);
        }

        public new bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;

            ListMethod next = () => base.RemoveAt(index);
            ListMethod last = () => base.Insert(index, item);

            AddHistoryData(last, next);
            next();
            return true;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            var matches = FindAllIndexMatches(match);

            ListMethod next = () =>
            {
                for (int i = matches.Count; --i >= 0; )
                    base.RemoveAt(matches[i].index);
            };
            ListMethod last = () =>
            {
                for (int i = 0; i < matches.Count; i++)
                    base.Insert(matches[i].index, matches[i].item);
            };

            AddHistoryData(last, next);
            next();

            return matches.Count;
        }

        public new void RemoveAt(int index)
        {
            var item = this[index];
            ListMethod last = () => base.Insert(index, item);
            ListMethod next = () => base.RemoveAt(index);

            AddHistoryData(last, next);
            next();
        }

        public new void RemoveRange(int index, int count)
        {
            var collection = GetRange(index, count);
            ListMethod last = () => base.InsertRange(index, collection);
            ListMethod next = () => base.RemoveRange(index, count);

            AddHistoryData(last, next);
            next();
        }

        public new void Reverse(int index, int count)
        {
            ListMethod last = () => base.Reverse(index, count);
            ListMethod next = () => base.Reverse(index, count);

            AddHistoryData(last, next);
            next();
        }

        public new void Reverse()
        {
            ListMethod last = () => base.Reverse();
            ListMethod next = () => base.Reverse();

            AddHistoryData(last, next);
            next();
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            var collection = GetRange(index, count);

            ListMethod last = () => base.InsertRange(index, collection);
            ListMethod next = () => base.Sort(index, count, comparer);

            AddHistoryData(last, next);
            next();
        }

        public new void Sort(Comparison<T> comparison)
        {
            var collection = new List<T>(this);

            ListMethod last = () => base.AddRange(collection);
            ListMethod next = () => base.Sort(comparison);

            AddHistoryData(last, next);
            next();
        }

        public new void Sort()
        {
            var collection = new List<T>(this);

            ListMethod last = () => base.AddRange(collection);
            ListMethod next = () => base.Sort();

            AddHistoryData(last, next);
            next();
        }

        public new void Sort(IComparer<T> comparer)
        {
            var collection = new List<T>(this);

            ListMethod last = () => base.AddRange(collection);
            ListMethod next = () => base.Sort(comparer);

            AddHistoryData(last, next);
            next();
        }

        private void AddHistoryData(ListMethod last, ListMethod next)
        {
            if (HistoryIndex < History.Count)
                History.RemoveRange(HistoryIndex, History.Count - HistoryIndex);

            History.Add(new State(last, next));
            HistoryIndex++;
        }

        public void Undo()
        {
            if (CanUndo)
            {
                if (HistoryIndex <= 0)
                    throw new InvalidOperationException();// Resources.ErrorNoCopyData);

                History[--HistoryIndex].Last();
                //OnUndoApplied(EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                if (HistoryIndex >= History.Count)
                    throw new InvalidOperationException();// Resources.ErrorNoRedoData);

                History[HistoryIndex++].Next();
                //OnRedoApplied(EventArgs.Empty);
            }
        }

        private struct State
        {
            public ListMethod Last
            {
                get;
                private set;
            }
            public ListMethod Next
            {
                get;
                private set;
            }

            public State(ListMethod last, ListMethod next)
            {
                Last = last ?? throw new ArgumentNullException(nameof(last));
                Next = next ?? throw new ArgumentNullException(nameof(next));
            }
        }
    }
}
