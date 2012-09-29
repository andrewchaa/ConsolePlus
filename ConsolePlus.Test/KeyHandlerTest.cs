using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ConsolePlus.Domain;
using NUnit.Framework;

namespace ConsolePlus.Test
{
    [TestFixture]
    public class KeyHandlerTest
    {
        private KeyHandler _handler;

        [SetUp]
        public void Before_Each_Test()
        {
            _handler = new KeyHandler();
        }

        [Test]
        public void Return_Key_Gets_Converted_To_Return_Character()
        {
            Assert.That(_handler.GetCharacterFrom(Key.Return), Is.EqualTo((char)13));
        }

        [Test]
        public void SHIFT_KEY_Should_Be_Ignored()
        {
            Assert.That(_handler.GetCharacterFrom(Key.LeftShift), Is.EqualTo(char.MinValue));
        }

    }
}
