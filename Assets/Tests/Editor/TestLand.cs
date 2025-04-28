using Core.Manager;
using NUnit.Framework;

namespace Tests.Editor
{
    public class TestLand
    {
        [Test]
        public void TestNumLandFirst()
        {
            Assert.AreEqual(GameManager.Instance.LandManager.TotalLand,
                GameManager.Instance.Configs.GlobalConfig.GetInt("Land_First"));
        }
        
        [Test]
        public void TestAddLand()
        {
            var oldNum = GameManager.Instance.LandManager.TotalLand;
            var price = GameManager.Instance.Configs.GlobalConfig.GetInt("Land_ExpansionPrice");
            GameManager.Instance.PlayerResources.AddGold(price);
            GameManager.Instance.ExpandLand();
            var newNum = GameManager.Instance.LandManager.TotalLand;
            Assert.AreEqual(oldNum + 1, newNum);
        }
    }
}