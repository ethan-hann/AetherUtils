using AetherUtils.Core.Extensions;
using AetherUtils.Core.Security;

namespace AetherUtils.Tests
{
    public class SecretQATests
    {
        [Test]
        public void CreateListTest()
        {
            List<SecretQa> list = [];

            for (var i = 0; i < 10; i++)
                list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

            Assert.That(list, Has.Count.EqualTo(10));
        }

        [Test]
        public void GetItemTest()
        {
            List<SecretQa> list = [];

            for (var i = 0; i < 10; i++)
                list.Add(new SecretQa($"Question {i}", $"Answer {i}"));

            list.RemoveAll(s => s.Answer.Equals("Question 2"));

            Assert.That(list[1].Answer, Is.EqualTo("Answer 1"));
        }
    }
}
