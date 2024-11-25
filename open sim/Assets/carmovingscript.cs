using UnityEngine;

namespace tsd
{
    public class CarBackAndForth : MonoBehaviour
    {
        public Transform waypoint1; // First waypoint
        public Transform waypoint2; // Second waypoint
        public float speed = 5f;    // Movement speed

        private Transform targetWaypoint; // The current target waypoint

        private void Start()
        {
            // Set the initial target to the second waypoint
            targetWaypoint = waypoint2;
        }

        private void Update()
        {
            // Move the car towards the target waypoint
            MoveCar();

            // Check if the car has reached the target waypoint
            if (HasReachedWaypoint())
            {
                // Snap the car to the waypoint to ensure precision
                transform.position = targetWaypoint.position;

                // Rotate the car 180 degrees
                transform.Rotate(0, 180, 0);

                // Switch to the opposite waypoint
                targetWaypoint = targetWaypoint == waypoint1 ? waypoint2 : waypoint1;
            }
        }

        private void MoveCar()
        {
            // Calculate direction to the target waypoint
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;

            // Move the car in the calculated direction
            transform.position += direction * speed * Time.deltaTime;
        }

        private bool HasReachedWaypoint()
        {
            // Check if the car is within a small distance of the target waypoint
            return Vector3.Distance(transform.position, targetWaypoint.position) <= 0.1f;
        }
    }
}
