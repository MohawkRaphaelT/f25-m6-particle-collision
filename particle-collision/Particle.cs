using System;
using System.Numerics;

namespace MohawkGame2D;

public class Particle
{
    Vector2 position;
    Vector2 velocity;
    int size;
    Color color;

    public void Setup()
    {
        // Give random position inside of screen from (0,0) to Window size
        position = Random.Vector2(Window.Size);
        // Create vector to move with
        Vector2 direction = Random.Direction();
        float speed = Random.Float(25, 75); // 25-75 pixels per second
        velocity = direction * speed;
        // Hard coded for now
        size = 3; // 3x3 pixels
        color = Color.Magenta;
    }

    public void Update(Obstacle[] obstacles)
    {
        // Move particle
        position += velocity * Time.DeltaTime;

        // Constrain to bounds of screen
        ConstrainToScreenBounds();

        // Collide with obstacles
        for (int i = 0; i < obstacles.Length; i++)
            CollideWithObstacle(obstacles[i]);

        // Draw particle
        Draw.LineSize = 0;
        Draw.LineColor = Color.Black;
        Draw.FillColor = color;
        Draw.Square(position, size);
    }

    void ConstrainToScreenBounds()
    {
        float particleL = position.X;
        float particleR = position.X + size;
        float particleT = position.Y;
        float particleB = position.Y + size;

        // left edge check; 0 is window left edge
        if (particleL <= 0)
        {
            // move particle to left edge of screen
            position.X = 0;
            // invert velocity to move back into screen
            velocity.X = -velocity.X;
        }

        if (particleR >= Window.Width)
        {
            position.X = position.X - size;
            velocity.X = -velocity.X;
        }

        if (particleT <= 0)
        {
            position.Y = 0;
            velocity.Y = -velocity.Y;
        }

        if (particleB >= Window.Height)
        {
            position.Y = Window.Height - size;
            velocity.Y = -velocity.Y;
        }
    }

    void CollideWithObstacle(Obstacle obstacle)
    {
        // Particles edges
        float particleL = position.X;
        float particleR = position.X + size;
        float particleT = position.Y;
        float particleB = position.Y + size;
        // Obstacle edges
        float obstacleL = obstacle.position.X;
        float obstacleR = obstacle.position.X + obstacle.size.X;
        float obstacleT = obstacle.position.Y;
        float obstacleB = obstacle.position.Y + obstacle.size.Y;

        // Check if particle is within all bounds of obstacle
        bool isParticleWithinR = particleL <= obstacleR;
        bool isParticleWithinL = particleR >= obstacleL;
        bool isParticleWithinT = particleB >= obstacleT;
        bool isParticleWithinB = particleT <= obstacleB;

        // is colliding at all
        if (isParticleWithinL && isParticleWithinR && isParticleWithinT && isParticleWithinB)
        {
            // compare to centre of obstacle, snap to closes edge

            if (particleB <= obstacleB && particleT >= obstacleT)
            {
                velocity.X = -velocity.X;
            }

            if (particleL >= obstacleL && particleR <= obstacleR)
            {
                velocity.Y = -velocity.Y;
            }
        }
    }
}
