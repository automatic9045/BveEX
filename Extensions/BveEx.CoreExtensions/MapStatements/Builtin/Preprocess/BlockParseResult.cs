using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Extensions.MapStatements.Builtin.Preprocess
{
    internal struct BlockParseResult
    {
        public static readonly BlockParseResult Default = new BlockParseResult(NestOperationMode.Continue, false, false);

        public NestOperationMode NestOperation { get; }
        public bool IgnoreMyself { get; }
        public bool IgnoreFollowing { get; }

        private BlockParseResult(NestOperationMode nestOperation, bool ignoreMyself, bool ignoreFollowing)
        {
            NestOperation = nestOperation;
            IgnoreMyself = ignoreMyself;
            IgnoreFollowing = ignoreFollowing;
        }

        public static BlockParseResult Effective(NestOperationMode nestOperation, bool ignoreFollowing)
            => new BlockParseResult(nestOperation, false, ignoreFollowing);

        public static BlockParseResult Ignored(NestOperationMode nestOperation)
            => new BlockParseResult(nestOperation, true, true);


        internal enum NestOperationMode
        {
            Continue,
            Begin,
            End,
        }
    }
}
