using System.Collections;
using System.Collections.Generic;
using System.IO;
using Algebra;
using Algebra.Atoms;
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
            string expression = name;
            IExpression expected = IExpression.VariableFrom(name);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesConstants()
        {
            // ARANGE
            string expression = "987654321.5";
            IExpression expected = IExpression.ConstantFrom((Rational)987654321.5M);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesFractionConstants()
        {
            // ARANGE
            string expression = "98765/24";
            IExpression expected = IExpression.ConstantFrom((Rational)98765 / 24);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesAddition()
        {
            // ARANGE
            string expression = "x + 1";
            IExpression expected = IExpression.X + 1;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMultiplication()
        {
            // ARANGE
            string expression = "x * y";
            IExpression expected = IExpression.X * IExpression.Y;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSubtraction()
        {
            // ARANGE
            string expression = "x - 50";
            IExpression expected = IExpression.X + (-50);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDivision()
        {
            // ARANGE
            string expression = "x / y";
            IExpression expected = IExpression.X / IExpression.Y;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesExponentiation()
        {
            // ARANGE
            string expression = "x ^ y";
            IExpression expected = IExpression.Pow(IExpression.X, IExpression.Y);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesBraces()
        {
            // ARANGE
            string expression = "(x + y) * 5";
            IExpression expected = (IExpression.X + IExpression.Y) * 5;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSingleNegation()
        {
            // ARANGE
            string expression = "-5";
            IExpression expected = -1 * 5;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesDoubleNegation()
        {
            // ARANGE
            string expression = "--5";
            IExpression expected = -1 * (-1 * 5);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBraceless()
        {
            // ARANGE
            string expression = "ln 5";
            IExpression expected = IExpression.LnOf(IExpression.ConstantFrom(5));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBraces()
        {
            // ARANGE
            string expression = "ln(52)";
            IExpression expected = IExpression.LnOf(IExpression.ConstantFrom(52));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLog()
        {
            // ARANGE
            string expression = "Log (y, x) ";
            IExpression expected = IExpression.LogOf(IExpression.Y, IExpression.X);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLogCapital()
        {
            // ARANGE
            string expression = "LOG (152, x) ";
            IExpression expected = IExpression.LogOf(IExpression.ConstantFrom(152), IExpression.X);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedLnBraceless()
        {
            // ARANGE
            string expression = "ln ln 15";
            IExpression expected = IExpression.LnOf(IExpression.LnOf(IExpression.ConstantFrom(15)));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesLnBracelessAsOnlyNextLeaf()
        {
            // ARANGE
            string expression = "y * ln 5 * x + 3";
            IExpression expected = (IExpression.Y * IExpression.LnOf(5) * IExpression.X) + 3;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraceless()
        {
            // ARANGE
            string expression = "sign 5";
            IExpression expected = IExpression.SignOf(IExpression.ConstantFrom(5));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesSignBraces()
        {
            // ARANGE
            string expression = "sign(529)";
            IExpression expected = IExpression.SignOf(IExpression.ConstantFrom(529));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraceless()
        {
            // ARANGE
            string expression = "sign sign 145";
            IExpression expected = IExpression.SignOf(IExpression.SignOf(IExpression.ConstantFrom(145)));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedSignBraces()
        {
            // ARANGE
            string expression = "sign(sign(1555))";
            IExpression expected = IExpression.SignOf(IExpression.SignOf(IExpression.ConstantFrom(1555)));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinConstants()
        {
            // ARANGE
            string expression = "min(1, 2)";
            IExpression expected = IExpression.Min(1, 2);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinVariables()
        {
            // ARANGE
            string expression = "min(x, y)";
            IExpression expected = IExpression.Min(IExpression.X, IExpression.Y);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinAddition()
        {
            // ARANGE
            string expression = "min(x + y, y)";
            IExpression expected = IExpression.Min(IExpression.X + IExpression.Y, IExpression.Y);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMultiplication()
        {
            // ARANGE
            string expression = "min(x * y, y * z)";
            IExpression expected = IExpression.Min(IExpression.X * IExpression.Y, IExpression.Y * IExpression.Z);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxConstants()
        {
            // ARANGE
            string expression = "max(1, 2)";
            IExpression expected = IExpression.Max(1, 2);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxVariables()
        {
            // ARANGE
            string expression = "max(x, y)";
            IExpression expected = IExpression.Max(IExpression.X, IExpression.Y);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxAddition()
        {
            // ARANGE
            string expression = "max(x + y, y)";
            IExpression expected = IExpression.Max(IExpression.X + IExpression.Y, IExpression.Y);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMaxMultiplication()
        {
            // ARANGE
            string expression = "max(x * y, y * z)";
            IExpression expected = IExpression.Max(IExpression.X * IExpression.Y, IExpression.Y * IExpression.Z);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesMinMaxMultiplication()
        {
            // ARANGE
            string expression = "min (1, x) * max(x * y, y * z)";
            IExpression expected = IExpression.Min(1, IExpression.X) * IExpression.Max(IExpression.X * IExpression.Y, IExpression.Y * IExpression.Z);

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ParsesChainedBraces()
        {
            // ARANGE
            string expression = "(((15)))";
            IExpression expected = 15;

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_ThrowsOnUnknownVariables([Values("X", "Y", "Z", "W", "V", "val", "t")] string name)
        {
            // ARANGE
            string expression = name;
            ISet<string> variables = new HashSet<string>();

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression, variables),
                  Throws.TypeOf<InvalidDataException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces1()
        {
            // ARANGE
            string expression = "(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces2()
        {
            // ARANGE
            string expression = "12(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces3()
        {
            // ARANGE
            string expression = "(192)(";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnTooManyOpenBraces4()
        {
            // ARANGE
            string expression = "((1223)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutLHS()
        {
            // ARANGE
            string expression = "+ 9";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnAdditionWithoutRHS()
        {
            // ARANGE
            string expression = "1 +";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutLHS()
        {
            // ARANGE
            string expression = "* 8";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMultiplicationWithoutRHS()
        {
            // ARANGE
            string expression = "17 *";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutLHS()
        {
            // ARANGE
            string expression = "^ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnExponentiationWithoutRHS()
        {
            // ARANGE
            string expression = "x ^";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnSubtractionWithoutRHS()
        {
            // ARANGE
            string expression = "x -";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutLHS()
        {
            // ARANGE
            string expression = "/ x";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnDivisionWithoutRHS()
        {
            // ARANGE
            string expression = "x /";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithoutArguments()
        {
            // ARANGE
            string expression = "x + ln";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnLnWithTooManyArguments()
        {
            // ARANGE
            string expression = "7 + ln(x, y)";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ThrowsOnMinWithoutBraces()
        {
            // ARANGE
            string expression = "7 + min x, y";

            // ACT

            // ASSERT
            Assert.That(() => Parser.Parse(expression),
                  Throws.TypeOf<SyntaxException>());
        }

        [Test]
        public void Parser_ParsesDifferentFunctions()
        {
            // ARANGE
            string expression = "sign(ln 3883)";
            IExpression expected = IExpression.SignOf(IExpression.LnOf(IExpression.ConstantFrom(3883)));

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure1()
        {
            // ARANGE
            IExpression expected = IExpression.Pow(IExpression.Y, (Rational)3971 / 9748);
            string expression = "((y ^ 3971/9748))";

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure2()
        {
            // ARANGE
            IExpression expected = (IExpression.Pow(((IExpression.X + IExpression.Z + IExpression.ConstantFrom((Rational)(18162171) / (22852115))) * IExpression.ConstantFrom((Rational)(310824508140) / (806613624271)) * IExpression.Pow(IExpression.Pow(IExpression.Pow(IExpression.ConstantFrom((Rational)(3271) / (5692)), IExpression.Pow(IExpression.ConstantFrom((Rational)(3875) / (6319)), IExpression.Z)), IExpression.LnOf((IExpression.Y * IExpression.Z * IExpression.ConstantFrom((Rational)(7876848) / (551214185))))), IExpression.ConstantFrom((Rational)(7481) / (15820))) * IExpression.ConstantFrom((Rational)(1077) / (2105)) * IExpression.LnOf(IExpression.Pow(IExpression.ConstantFrom((Rational)(-31938485) / (179632821)), IExpression.ConstantFrom((Rational)(3719) / (3898)))) * IExpression.Pow(IExpression.ConstantFrom((Rational)(34060164457463332) / (84917266513976971)), IExpression.ConstantFrom((Rational)(7170) / (8081)))), IExpression.ConstantFrom((Rational)(3757) / (7268))) + IExpression.ConstantFrom((Rational)(2704) / (8117)) + IExpression.Pow((IExpression.Pow(IExpression.Z, IExpression.Pow(((IExpression.Y * IExpression.ConstantFrom((Rational)(281756499029) / (4278581823008))) + IExpression.X + IExpression.LnOf(IExpression.Pow(IExpression.ConstantFrom((Rational)(107) / (91937)), IExpression.ConstantFrom((Rational)(23485) / (60351)))) + IExpression.ConstantFrom((Rational)(241903) / (1023370))), IExpression.Pow(IExpression.ConstantFrom((Rational)(-84694382) / (48380467)), IExpression.ConstantFrom((Rational)(132483) / (137719))))) + IExpression.Pow(IExpression.Pow((IExpression.Pow(IExpression.ConstantFrom((Rational)(3445) / (5332)), IExpression.ConstantFrom((Rational)(2774) / (27105))) * IExpression.ConstantFrom((Rational)(1990944) / (93080449))), (IExpression.X + IExpression.Y + IExpression.ConstantFrom((Rational)(9777) / (10613)) + (IExpression.Pow(IExpression.ConstantFrom((Rational)(17643) / (49516)), (IExpression.Y * IExpression.ConstantFrom((Rational)(1172263) / (22515807)))) * IExpression.Pow(IExpression.Z, IExpression.ConstantFrom((Rational)(1262) / (8407))) * IExpression.ConstantFrom((Rational)(3121) / (3977))))), IExpression.ConstantFrom((Rational)(-56263742) / (634120667))) + IExpression.Pow(IExpression.Pow(IExpression.Pow(IExpression.X, IExpression.Pow(IExpression.ConstantFrom((Rational)(6957) / (15283)), IExpression.Y)), IExpression.ConstantFrom((Rational)(2435) / (4603))), IExpression.ConstantFrom((Rational)(90859500) / (183100176133))) + IExpression.Pow(IExpression.Pow(IExpression.Pow(IExpression.LnOf(((IExpression.X + IExpression.ConstantFrom((Rational)(1684097) / (2595208))) * IExpression.Pow(IExpression.ConstantFrom((Rational)(3541) / (4870)), IExpression.Z) * IExpression.ConstantFrom((Rational)(-351601137) / (516062869)))), IExpression.LnOf((IExpression.ConstantFrom((Rational)(520875825076) / (626696076699)) + IExpression.Pow(IExpression.ConstantFrom((Rational)(493) / (7544)), IExpression.ConstantFrom((Rational)(4035) / (4984)))))), (IExpression.Pow(IExpression.Y, IExpression.Y) * IExpression.Pow(IExpression.LnOf(IExpression.Pow(IExpression.ConstantFrom((Rational)(1845) / (4184)), IExpression.ConstantFrom((Rational)(4390) / (5839)))), IExpression.ConstantFrom((Rational)(529) / (9484))))), IExpression.ConstantFrom((Rational)(35351) / (78776))) + IExpression.Pow(IExpression.X, IExpression.Z) + IExpression.ConstantFrom((Rational)(9241) / (17113)) + IExpression.Pow(IExpression.ConstantFrom((Rational)(4733) / (11013)), IExpression.Y) + IExpression.Pow(IExpression.LnOf(IExpression.Pow(IExpression.Y, IExpression.Z)), IExpression.Pow(IExpression.ConstantFrom((Rational)(8413) / (10191)), IExpression.LnOf(IExpression.Pow(IExpression.ConstantFrom((Rational)(1784) / (7051)), IExpression.Pow(IExpression.X, IExpression.Y))))) + IExpression.ConstantFrom((Rational)(3572896512839) / (32129243331984))), IExpression.ConstantFrom((Rational)(4071) / (4378))));
            string expression = "((((x + z + 18162171/22852115) * 310824508140/806613624271 * (((3271/5692 ^ (3875/6319 ^ z)) ^ ln (y * z * 7876848/551214185)) ^ 7481/15820) * 1077/2105 * ln (-31938485/179632821 ^ 3719/3898) * (34060164457463332/84917266513976971 ^ 7170/8081)) ^ 3757/7268) + 2704/8117 + (((z ^ (((y * 281756499029/4278581823008) + x + ln (107/91937 ^ 23485/60351) + 241903/1023370) ^ (-84694382/48380467 ^ 132483/137719))) + ((((3445/5332 ^ 2774/27105) * 1990944/93080449) ^ (x + y + 9777/10613 + ((17643/49516 ^ (y * 1172263/22515807)) * (z ^ 1262/8407) * 3121/3977))) ^ -56263742/634120667) + (((x ^ (6957/15283 ^ y)) ^ 2435/4603) ^ 90859500/183100176133) + (((ln ((x + 1684097/2595208) * (3541/4870 ^ z) * -351601137/516062869) ^ ln (520875825076/626696076699 + (493/7544 ^ 4035/4984))) ^ ((y ^ y) * (ln (1845/4184 ^ 4390/5839) ^ 529/9484))) ^ 35351/78776) + (x ^ z) + 9241/17113 + (4733/11013 ^ y) + (ln (y ^ z) ^ (8413/10191 ^ ln (1784/7051 ^ (x ^ y)))) + 3572896512839/32129243331984) ^ 4071/4378))";

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Parser_FuzzerGeneratedFailure3()
        {
            // ARANGE
            IExpression expected = (IExpression.Z * IExpression.Pow(IExpression.Y, IExpression.ConstantFrom((Rational)(2) / (1))) * IExpression.SignOf((IExpression.LnOf(IExpression.ConstantFrom((Rational)(4971) / (5138))) + IExpression.ConstantFrom((Rational)(3996) / (4441)) + IExpression.Pow(IExpression.X, IExpression.Pow(IExpression.ConstantFrom((Rational)(2401) / (4209)), IExpression.SignOf(IExpression.Pow(IExpression.LnOf(IExpression.Pow(IExpression.ConstantFrom((Rational)(39449) / (47989)), IExpression.SignOf(IExpression.Z))), IExpression.Pow(((IExpression.Z + IExpression.ConstantFrom((Rational)(815) / (6387)) + IExpression.Y) * IExpression.ConstantFrom((Rational)(1894) / (5563))), IExpression.ConstantFrom((Rational)(13524) / (15523))))))) + IExpression.LnOf(IExpression.SignOf(IExpression.ConstantFrom((Rational)(1368) / (19661)))))));
            string expression = "(z * (y ^ 2) * sign (ln 4971/5138 + 3996/4441 + (x ^ (2401/4209 ^ sign (ln (39449/47989 ^ sign z) ^ (((z + 815/6387 + y) * 1894/5563) ^ 13524/15523)))) + ln sign 1368/19661))";

            // ACT
            IExpression result = Parser.Parse(expression);

            // ASSERT
            Assert.AreEqual(expected, result);
        }
    }
    
}
