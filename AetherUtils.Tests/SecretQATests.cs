using AetherUtils.Core.Extensions;
using AetherUtils.Core.Security;

namespace AetherUtils.Tests
{
    public class SecretQATests
    {
        [Test]
        public void CreateListTest()
        {
            SecretQAList list = new SecretQAList();

            for (int i = 0; i < 10; i++)
                list.Add($"Question {i}", $"Answer {i}");

            Assert.That(list.Count, Is.EqualTo(10));
        }

        [Test]
        public void RemoveItemTest()
        {
            SecretQAList list = new SecretQAList();

            for (int i = 0; i < 10; i++)
                list.Add($"Question {i}", $"Answer {i}");

            list.Remove("Question 2");

            Assert.That(list.Count, Is.EqualTo(9)); //Should be 9 items after removing the question above.
        }

        [Test]
        public void GetItemTest()
        {
            SecretQAList list = new SecretQAList();

            for (int i = 0; i < 10; i++)
                list.Add($"Question {i}", $"Answer {i}");

            list.Remove("Question 2");

            Assert.That(list[1].Answer.FromSecureString(), Is.EqualTo("Answer 1"));
        }
    }
}
