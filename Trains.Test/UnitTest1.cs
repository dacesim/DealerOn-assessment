using NUnit.Framework;
using Trains;

namespace Trains.Test
{
    [TestFixture]
    public class TrainsTest
    {
        [Test]
        public void CalculateDistance_ShouldReturnCorrectDistance()
        {
            // Arrange
            var trains = new Trains(new List<string> { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" });

            // Act
            int distance1 = trains.CalculateDistance("ABC");
            int distance2 = trains.CalculateDistance("AD");
            int distance3 = trains.CalculateDistance("ADC");
            int distance4 = trains.CalculateDistance("AEBCD");
            int distance5 = trains.CalculateDistance("AED");

            // Assert
            Assert.AreEqual(9, distance1);
            Assert.AreEqual(5, distance2);
            Assert.AreEqual(13, distance3);
            Assert.AreEqual(22, distance4);
            Assert.AreEqual(-1, distance5); // No such route
        }

        [Test]
        public void CountRoutes_ShouldReturnCorrectCount()
        {
            // Arrange
            var trains = new Trains(new List<string> { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" });

            // Act
            int count1 = trains.CountRoutes('C', 'C', 3, "Stops");
            int count2 = trains.CountRoutes('C', 'C', 30, "Distance");

            // Assert
            Assert.AreEqual(2, count1);
            Assert.AreEqual(7, count2);
        }

        [Test]
        public void CountRoutesWithExactStops_ShouldReturnCorrectCount()
        {
            // Arrange
            var trains = new Trains(new List<string> { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" });

            // Act
            int count1 = trains.CountRoutesWithExactStops('A', 'C', 4);

            // Assert
            Assert.AreEqual(3, count1);
        }

        [Test]
        public void ShortestRouteDistance_ShouldReturnCorrectDistance()
        {
            // Arrange
            var trains = new Trains(new List<string> { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" });

            // Act
            int distance1 = trains.ShortestRouteDistance('A', 'C');
            int distance2 = trains.ShortestRouteDistance('B', 'B');
            int distance3 = trains.ShortestRouteDistance('A', 'D');
            int distance4 = trains.ShortestRouteDistance('A', 'E');

            // Assert
            Assert.AreEqual(9, distance1);
            Assert.AreEqual(9, distance2);
            Assert.AreEqual(5, distance3);
            Assert.AreEqual(7, distance4);
        }
    }
}
