using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helper;

namespace MushROMs.Assembler
{
    public class Log
    {
        internal Dictionary<MessageCode, string> MessageCodeDictionary
        {
            get;
            private set;
        }
        internal Dictionary<WarningCode, string> WarningCodeDictionary
        {
            get;
            private set;
        }
        internal Dictionary<ErrorCode, string> ErrorCodeDictionary
        {
            get;
            private set;
        }

        public TextWriter MessageOutput
        {
            get;
            set;
        }

        public TextWriter WarningOutput
        {
            get;
            set;
        }

        public TextWriter ErrorOutput
        {
            get;
            set;
        }

        public Log()
            : this(Console.Out, Console.Out, Console.Error)
        { }
        public Log(TextWriter messageOutput)
            : this(messageOutput, messageOutput, messageOutput)
        { }
        public Log(TextWriter messageOutput, TextWriter errorOutput) :
            this(messageOutput, messageOutput, errorOutput)
        { }
        public Log(TextWriter messageOutput, TextWriter warningOutput, TextWriter errorOutput)
        {
            MessageOutput = messageOutput;
            WarningOutput = warningOutput;
            ErrorOutput = errorOutput;

            MessageCodeDictionary = new Dictionary<MessageCode, string>();
            WarningCodeDictionary = new Dictionary<WarningCode, string>();
            ErrorCodeDictionary = new Dictionary<ErrorCode, string>();
        }

        public void AddMessage(MessageCode code, params object[] args)
        {
            if (MessageOutput == null)
                return;

            if (MessageCodeDictionary.ContainsKey(code))
            {
                string format = MessageCodeDictionary[code];
                string output = SR.GetString(format, args);

                MessageOutput.WriteLine(output);
            }
        }

        public void AddWarning(WarningCode code, params object[] args)
        {
            if (WarningOutput == null)
                return;

            if (WarningCodeDictionary.ContainsKey(code))
            {
                var format = WarningCodeDictionary[code];
                var output = SR.GetString(format, args);
                WarningOutput.WriteLine(output);
            }
        }

        public void AddError(ErrorCode code, params object[] args)
        {
            if (ErrorOutput == null)
                return;

            if (ErrorCodeDictionary.ContainsKey(code))
            {
                var format = ErrorCodeDictionary[code];
                var output = SR.GetString(format, args);
                ErrorOutput.WriteLine(output);
            }
            else
            {
                var format = ErrorCodeDictionary[0];
                var sb = new StringBuilder(SR.GetString(format, (int)code));
                foreach (var arg in args)
                {
                    sb.Append(arg);
                    sb.Append(", ");
                }
                ErrorOutput.WriteLine(sb.ToString());
            }
        }
    }
}
