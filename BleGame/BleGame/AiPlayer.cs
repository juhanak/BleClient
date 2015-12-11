using BleGame.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleGame
{
    class AiPlayer
    {
        private SpaceShipControl aiControl;
        private SpaceShipControl opponentControl;

        public AiPlayer(SpaceShipControl ai, SpaceShipControl opponent)
        {
            aiControl = ai;
            opponentControl = opponent;
        }

        public void Update(double angleTurnMagnitude, double thrustMagnitude)
        {
            double xDiff = Math.Abs(aiControl.X - opponentControl.X);
            double yDiff = Math.Abs(aiControl.Y - opponentControl.Y);
            double desiredAngleInRadians = Math.Atan(yDiff / xDiff); // Between 0 and PI/2

            if (aiControl.X <= opponentControl.X)
            {
                if (aiControl.Y < opponentControl.Y)
                {
                    desiredAngleInRadians += Math.PI / 2;
                }
                else
                {
                    desiredAngleInRadians = Math.PI / 2 - desiredAngleInRadians;
                }
            }
            else if (aiControl.X > opponentControl.X)
            {
                if (aiControl.Y < opponentControl.Y)
                {
                    desiredAngleInRadians = Math.PI / 2 - desiredAngleInRadians + Math.PI;
                }
                else
                {
                    desiredAngleInRadians += Math.PI * 3 / 2; 
                }
            }

            double desiredAngleInDegrees = desiredAngleInRadians * 180 / Math.PI;
            double absDeltaBetweenAngles = Math.Abs(aiControl.Angle - desiredAngleInDegrees);
            double angleDeltaToApply = (absDeltaBetweenAngles < angleTurnMagnitude) ? absDeltaBetweenAngles : angleTurnMagnitude;

            if (desiredAngleInDegrees < aiControl.Angle)
            {
                angleDeltaToApply *= -1;
            }

            if (absDeltaBetweenAngles > 180d)
            {
                angleDeltaToApply *= -1;
            }

            aiControl.Angle += angleDeltaToApply;

            aiControl.ApplyThrustBasedOnAngle(thrustMagnitude);
        }
    }
}
