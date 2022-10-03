using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLang.AbstractTree.Statements.Expressions
{
    internal class OrExpression : BinaryExpression
    {
        public OrExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
        {
        }

        protected override string Delimeter => "or";
    }


    internal class XorExpression : BinaryExpression
    {
        public XorExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
        {
        }

        protected override string Delimeter => "xor";
    }

    internal class AndExpression : BinaryExpression
    {
        public AndExpression(IExpression left, IExpression right, SourcePosition sourcePosition) : base(left, right, sourcePosition)
        {
        }

        protected override string Delimeter => "and";
    }
}
