using Helper;

namespace MushROMs
{
    public class EditorDictionary : PathDictionary<IEditor>
    {
        private MasterEditor _owner;

        public MasterEditor Owner => _owner;
        public EditorDictionary(MasterEditor owner)
        {
            _owner = owner;
        }
    }
}
