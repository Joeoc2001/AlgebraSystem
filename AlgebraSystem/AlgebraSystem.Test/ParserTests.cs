using System.Collections;
using System.Collections.Generic;
using System.IO;
using Algebra;
using Algebra.Operations;
using Algebra.Parsing;
using NUnit.Framework;
using Rationals;

namespace AlgebraTests
{
    public class ParserTests
    {
        [Test]
        public void Parser_ParsesVariables([Values("X", "Y", "Z")] string name)
        {
            // ARANGE
            string equation = name;
            Variable expected = new Variable(name);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesConstants()
        {
            // ARANGE
            string equation = "987654321.5";
            Constant expected = Constant.From((Rational)987654321.5M);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesFractionConstants()
        {
            // ARANGE
            string equation = "98765/24";
            Constant expected = Constant.From((Rational)98765 / 24);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesAddition()
        {
            // ARANGE
            string equation = "x + 1";
            Expression expected = Variable.X + 1;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMultiplication()
        {
            // ARANGE
            string equation = "x * y";
            Expression expected = Variable.X * Variable.Y;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSubtraction()
        {
            // ARANGE
            string equation = "x - 50";
            Expression expected = Variable.X + (-50);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDivision()
        {
            // ARANGE
            string equation = "x / 2";
            Expression expected = Variable.X / 2;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesExponentiation()
        {
            // ARANGE
            string equation = "x ^ y";
            Expression expected = Expression.Pow(Variable.X, Variable.Y);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesBraces()
        {
            // ARANGE
            string equation = "(x + y) * 5";
            Expression expected = (Variable.X + Variable.Y) * 5;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSingleNegation()
        {
            // ARANGE
            string equation = "-5";
            Expression expected = -1 * 5;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDoubleNegation()
        {
            // ARANGE
            string equation = "--5";
            Expression expected = -1 * (-1 * 5);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogBraceless()
        {
            // ARANGE
            string equation = "ln 5";
            Expression expected = Expression.LnOf(Constant.From(5));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogBraces()
        {
            // ARANGE
            string equation = "log(52)";
            Expression expected = Expression.LnOf(Constant.From(52));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogCapital()
        {
            // ARANGE
            string equation = "LOG 152 ";
            Expression expected = Expression.LnOf(Constant.From(152));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedLogBraceless()
        {
            // ARANGE
            string equation = "log ln 15";
            Expression expected = Expression.LnOf(Expression.LnOf(Constant.From(15)));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogBracelessAsOnlyNextLeaf()
        {
            // ARANGE
            string equation = "y * ln 5 * x + 3";
            Expression expected = (Variable.Y * Expression.LnOf(5) * Variable.X) + 3;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraceless()
        {
            // ARANGE
            string equation = "sign 5";
            Expression expected = Expression.SignOf(Constant.From(5));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraces()
        {
            // ARANGE
            string equation = "sign(529)";
            Expression expected = Expression.SignOf(Constant.From(529));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraceless()
        {
            // ARANGE
            string equation = "sign sign 145";
            Expression expected = Expression.SignOf(Expression.SignOf(Constant.From(145)));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraces()
        {
            // ARANGE
            string equation = "sign(sign(1555))";
            Expression expected = Expression.SignOf(Expression.SignOf(Constant.From(1555)));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinConstants()
        {
            // ARANGE
            string equation = "min(1, 2)";
            Expression expected = Expression.Min(1, 2);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinVariables()
        {
            // ARANGE
            string equation = "min(x, y)";
            Expression expected = Expression.Min(Variable.X, Variable.Y);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinAddition()
        {
            // ARANGE
            string equation = "min(x + y, y)";
            Expression expected = Expression.Min(Variable.X + Variable.Y, Variable.Y);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMultiplication()
        {
            // ARANGE
            string equation = "min(x * y, y * z)";
            Expression expected = Expression.Min(Variable.X * Variable.Y, Variable.Y * Variable.Z);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxConstants()
        {
            // ARANGE
            string equation = "max(1, 2)";
            Expression expected = Expression.Max(1, 2);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxVariables()
        {
            // ARANGE
            string equation = "max(x, y)";
            Expression expected = Expression.Max(Variable.X, Variable.Y);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxAddition()
        {
            // ARANGE
            string equation = "max(x + y, y)";
            Expression expected = Expression.Max(Variable.X + Variable.Y, Variable.Y);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxMultiplication()
        {
            // ARANGE
            string equation = "max(x * y, y * z)";
            Expression expected = Expression.Max(Variable.X * Variable.Y, Variable.Y * Variable.Z);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMaxMultiplication()
        {
            // ARANGE
            string equation = "min (1, 3) * max(x * y, y * z)";
            Expression expected = Expression.Min(1, 3) * Expression.Max(Variable.X * Variable.Y, Variable.Y * Variable.Z);

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedBraces()
        {
            // ARANGE
            string equation = "(((15)))";
            Expression expected = 15;

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ThrowsOnUnknownVariables([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            string equation = name;
            ISet<string> variables = new HashSet<string>();

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation, variables),
                  Throws.TypeOf<InvalidDataException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces1()
        {
            // ARANGE
            string equation = "(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces2()
        {
            // ARANGE
            string equation = "12(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces3()
        {
            // ARANGE
            string equation = "(192)(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces4()
        {
            // ARANGE
            string equation = "((1223)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutLHS()
        {
            // ARANGE
            string equation = "+ 9";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutRHS()
        {
            // ARANGE
            string equation = "1 +";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutLHS()
        {
            // ARANGE
            string equation = "* 8";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutRHS()
        {
            // ARANGE
            string equation = "17 *";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutLHS()
        {
            // ARANGE
            string equation = "^ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutRHS()
        {
            // ARANGE
            string equation = "x ^";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnSubtractionWithoutRHS()
        {
            // ARANGE
            string equation = "x -";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutLHS()
        {
            // ARANGE
            string equation = "/ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutRHS()
        {
            // ARANGE
            string equation = "x /";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithoutArguments()
        {
            // ARANGE
            string equation = "x + ln";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithTooManyArguments()
        {
            // ARANGE
            string equation = "7 + ln(x, y)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMinWithoutBraces()
        {
            // ARANGE
            string equation = "7 + min x, y";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(equation),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ParsesDifferentFunctions()
        {
            // ARANGE
            string equation = "sign(ln 3883)";
            Expression expected = Expression.SignOf(Expression.LnOf(Constant.From(3883)));

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure1()
        {
            // ARANGE
            Expression expected = Expression.Pow(Variable.Y, (Rational)3971 / 9748);
            string equation = "((y ^ 3971/9748))";

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure2()
        {
            // ARANGE
            Expression expected = (Expression.Pow(((Variable.X + Variable.Z + Constant.From((Rational)(18162171) / (22852115))) * Constant.From((Rational)(310824508140) / (806613624271)) * Expression.Pow(Expression.Pow(Expression.Pow(Constant.From((Rational)(3271) / (5692)), Expression.Pow(Constant.From((Rational)(3875) / (6319)), Variable.Z)), Expression.LnOf((Variable.Y * Variable.Z * Constant.From((Rational)(7876848) / (551214185))))), Constant.From((Rational)(7481) / (15820))) * Constant.From((Rational)(1077) / (2105)) * Expression.LnOf(Expression.Pow(Constant.From((Rational)(-31938485) / (179632821)), Constant.From((Rational)(3719) / (3898)))) * Expression.Pow(Constant.From((Rational)(34060164457463332) / (84917266513976971)), Constant.From((Rational)(7170) / (8081)))), Constant.From((Rational)(3757) / (7268))) + Constant.From((Rational)(2704) / (8117)) + Expression.Pow((Expression.Pow(Variable.Z, Expression.Pow(((Variable.Y * Constant.From((Rational)(281756499029) / (4278581823008))) + Variable.X + Expression.LnOf(Expression.Pow(Constant.From((Rational)(107) / (91937)), Constant.From((Rational)(23485) / (60351)))) + Constant.From((Rational)(241903) / (1023370))), Expression.Pow(Constant.From((Rational)(-84694382) / (48380467)), Constant.From((Rational)(132483) / (137719))))) + Expression.Pow(Expression.Pow((Expression.Pow(Constant.From((Rational)(3445) / (5332)), Constant.From((Rational)(2774) / (27105))) * Constant.From((Rational)(1990944) / (93080449))), (Variable.X + Variable.Y + Constant.From((Rational)(9777) / (10613)) + (Expression.Pow(Constant.From((Rational)(17643) / (49516)), (Variable.Y * Constant.From((Rational)(1172263) / (22515807)))) * Expression.Pow(Variable.Z, Constant.From((Rational)(1262) / (8407))) * Constant.From((Rational)(3121) / (3977))))), Constant.From((Rational)(-56263742) / (634120667))) + Expression.Pow(Expression.Pow(Expression.Pow(Variable.X, Expression.Pow(Constant.From((Rational)(6957) / (15283)), Variable.Y)), Constant.From((Rational)(2435) / (4603))), Constant.From((Rational)(90859500) / (183100176133))) + Expression.Pow(Expression.Pow(Expression.Pow(Expression.LnOf(((Variable.X + Constant.From((Rational)(1684097) / (2595208))) * Expression.Pow(Constant.From((Rational)(3541) / (4870)), Variable.Z) * Constant.From((Rational)(-351601137) / (516062869)))), Expression.LnOf((Constant.From((Rational)(520875825076) / (626696076699)) + Expression.Pow(Constant.From((Rational)(493) / (7544)), Constant.From((Rational)(4035) / (4984)))))), (Expression.Pow(Variable.Y, Variable.Y) * Expression.Pow(Expression.LnOf(Expression.Pow(Constant.From((Rational)(1845) / (4184)), Constant.From((Rational)(4390) / (5839)))), Constant.From((Rational)(529) / (9484))))), Constant.From((Rational)(35351) / (78776))) + Expression.Pow(Variable.X, Variable.Z) + Constant.From((Rational)(9241) / (17113)) + Expression.Pow(Constant.From((Rational)(4733) / (11013)), Variable.Y) + Expression.Pow(Expression.LnOf(Expression.Pow(Variable.Y, Variable.Z)), Expression.Pow(Constant.From((Rational)(8413) / (10191)), Expression.LnOf(Expression.Pow(Constant.From((Rational)(1784) / (7051)), Expression.Pow(Variable.X, Variable.Y))))) + Constant.From((Rational)(3572896512839) / (32129243331984))), Constant.From((Rational)(4071) / (4378))));
            string equation = "((((x + z + 18162171/22852115) * 310824508140/806613624271 * (((3271/5692 ^ (3875/6319 ^ z)) ^ ln (y * z * 7876848/551214185)) ^ 7481/15820) * 1077/2105 * ln (-31938485/179632821 ^ 3719/3898) * (34060164457463332/84917266513976971 ^ 7170/8081)) ^ 3757/7268) + 2704/8117 + (((z ^ (((y * 281756499029/4278581823008) + x + ln (107/91937 ^ 23485/60351) + 241903/1023370) ^ (-84694382/48380467 ^ 132483/137719))) + ((((3445/5332 ^ 2774/27105) * 1990944/93080449) ^ (x + y + 9777/10613 + ((17643/49516 ^ (y * 1172263/22515807)) * (z ^ 1262/8407) * 3121/3977))) ^ -56263742/634120667) + (((x ^ (6957/15283 ^ y)) ^ 2435/4603) ^ 90859500/183100176133) + (((ln ((x + 1684097/2595208) * (3541/4870 ^ z) * -351601137/516062869) ^ ln (520875825076/626696076699 + (493/7544 ^ 4035/4984))) ^ ((y ^ y) * (ln (1845/4184 ^ 4390/5839) ^ 529/9484))) ^ 35351/78776) + (x ^ z) + 9241/17113 + (4733/11013 ^ y) + (ln (y ^ z) ^ (8413/10191 ^ ln (1784/7051 ^ (x ^ y)))) + 3572896512839/32129243331984) ^ 4071/4378))";

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure3()
        {
            // ARANGE
            Expression expected = (Variable.Z * Expression.Pow(Variable.Y, Constant.From((Rational)(2) / (1))) * Expression.SignOf((Expression.LnOf(Constant.From((Rational)(4971) / (5138))) + Constant.From((Rational)(3996) / (4441)) + Expression.Pow(Variable.W, Expression.Pow(Constant.From((Rational)(2401) / (4209)), Expression.SignOf(Expression.Pow(Expression.LnOf(Expression.Pow(Constant.From((Rational)(39449) / (47989)), Expression.SignOf(Variable.Z))), Expression.Pow(((Variable.Z + Constant.From((Rational)(815) / (6387)) + Variable.Y) * Constant.From((Rational)(1894) / (5563))), Constant.From((Rational)(13524) / (15523))))))) + Expression.LnOf(Expression.SignOf(Constant.From((Rational)(1368) / (19661)))))));
            string equation = "(z * (y ^ 2) * sign (ln 4971/5138 + 3996/4441 + (w ^ (2401/4209 ^ sign (ln (39449/47989 ^ sign z) ^ (((z + 815/6387 + y) * 1894/5563) ^ 13524/15523)))) + ln sign 1368/19661))";

            // ACT
            Expression result = Parser.Parse(equation);

            // ASSERT
            Assert.AreEqual(expected, result);
        }
    }
    
}
