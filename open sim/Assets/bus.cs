using UnityEngine;

namespace tsd
{
    public class BusMovement : MonoBehaviour
    {
        public Transform[] waypoints;  // Array of waypoints defining the road path
        public float speed = 5f;       // Speed of the bus
        public float turnSpeed = 2f;   // Speed of rotation when turning

        private int currentWaypointIndex = 0; // Index of the current waypoint the bus is moving towards
        private bool isMovingForward = true;  // Whether the bus is moving forward or backward

        private void Start()
        {
            // Set initial position at the first waypoint
            transform.position = waypoints[0].position;

            // Align the bus’s rotation to the direction of the first road segment
            Vector3 directionToNextWaypoint = waypoints[1].position - waypoints[0].position;
            transform.rotation = Quaternion.LookRotation(directionToNextWaypoint);
        }

        private void Update()
        {
            MoveBus();

            // Check if the bus has reached the current waypoint
            if (HasReachedWaypoint())
            {
                // Move to the next waypoint if moving forward, or previous if moving backward
                if (isMovingForward)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
                else
                {
                    currentWaypointIndex = (currentWaypointIndex - 1 + waypoints.Length) % waypoints.Length;
                }

                // Reverse direction if the bus reaches the first or last waypoint
                if (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Length - 1)
                {
                    isMovingForward = !isMovingForward; // Reverse direction
                }
            }
        }

        private void MoveBus()
        {
            // Get the direction to the next waypoint
            Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
            direction.y = 0; // Ensure movement is on the X-Z plane (no vertical movement)

            // Smoothly rotate the bus to face the next waypoint
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Move the bus in the current direction
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }

        private bool HasReachedWaypoint()
        {
            // Check if the bus is close enough to the target waypoint
            return Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f;
        }
    }
}
