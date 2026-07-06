using System;
using task04;

namespace task04
{
    public class Fighter : ISpaceship
    {
        private int _speed = 100;
        private int _firePower = 50;
        private int _currentAngle = 0;
        public int Speed => _speed;
        public int FirePower => _firePower;
        public void MoveForward()
        {
        }
        public void Rotate(int angle)
        {
            _currentAngle = (_currentAngle + angle) % 360;
        }
        public void Fire()
        {
        }
    }
}
