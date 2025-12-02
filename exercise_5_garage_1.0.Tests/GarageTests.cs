using System;
using System.Drawing;
using System.Reflection.Metadata;
using static exercise_5_garage_1._0.Tests.GarageTests;

namespace exercise_5_garage_1._0.Tests
{
    public class GarageTests
    {
        [Fact]
        public void Indexer_SetAndGet_ShouldReturnCorrectVehicle()
        {
            // Arrange
            const int nrVehicles = 2;
            var garage = new Garage<Vehicle>(nrVehicles);
            var vehicle1 = new Boat();
            var vehicle2 = new Boat();
            garage[0] = vehicle1;
            garage[1] = vehicle2;

            // Act
            var retrievedVehicle = garage[1];

            // Assert
            Assert.Equal(vehicle2, retrievedVehicle);
        }

        [Fact]
        public void Enumerator_ShouldReturnAllVehicles()
        {
            // Arrange
            const int nrVehicles = 2;
            var garage = new Garage<Boat>(nrVehicles);
            var vehicle1 = new Boat();
            var vehicle2 = new Boat();
            garage[0] = vehicle1;
            garage[1] = vehicle2;

            // Act
            var vehicles = new List<Boat>(garage);

            // Assert
            Assert.Equal(nrVehicles, vehicles.Count);
            Assert.Contains(vehicle1, vehicles);
            Assert.Contains(vehicle2, vehicles);
        }

        [Fact]
        public void Indexer_Set_InvalidUpperIndex_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int nrVehicles = 1;
            var garage = new Garage<Vehicle>(nrVehicles);

            // Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => garage[1] = new Boat());

            // Assert
            Assert.Equal("ix ('1') must be less than '1'. (Parameter 'ix')\r\nActual value was 1.", exception.Message);
        }

        [Fact]
        public void Indexer_Set_InvalidLowerIndex_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int nrVehicles = 1;
            var garage = new Garage<Vehicle>(nrVehicles);

            // Act
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => garage[-1] = new Boat());

            // Assert
            Assert.Equal("ix ('-1') must be a non-negative value. (Parameter 'ix')\r\nActual value was -1.", exception.Message);
        }

        [Fact]
        public void Garage_ShouldHaveTheSpecifiedCapacity()
        {
            // Arrange
            const int nrVehicles = 3;

            // Act
            var garage = new Garage<Vehicle>(nrVehicles);

            // Assert
            Assert.Equal(nrVehicles, garage.Count());
        }

    }
}
