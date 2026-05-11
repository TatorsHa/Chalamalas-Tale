using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    void Start()
    {
        int roomRow = GridManager.Instance.currentRow;
        int roomCol = GridManager.Instance.currentCol;
        Debug.Log($"Loading room [{roomRow},{roomCol}]");

        // Check if the room has already been cleared by the player.
        // Only load enemies and collectibles if room hasn't been cleared yet
        CheckIfCleared(roomRow, roomCol);

        // Reposition player based on which side they entered from (Access the hitbox object, not the empty player parent)
        var player = PlayerHealth.Instance.transform;
        switch(GridManager.Instance.enteredFromSide)
        {
            case 0: player.transform.position = new Vector3(0.02f, -2.8f, 0); break; // entered from top, spawn at bottom
            case 1: player.transform.position = new Vector3(-7.21f, 0.51f, 0); break; // entered from right, spawn at left
            case 2: player.transform.position = new Vector3(0.17f, 3.47f, 0); break; // entered from bottom, spawn at top
            case 3: player.transform.position = new Vector3(6.76f, 0.46f, 0); break; // entered from left, spawn at right
        }
        //Debug.Log($"Player spawned at: {player.transform.position}");

        GameObject.Find("DoorTop").SetActive(   GridManager.Instance.IsOpen(roomRow, roomCol, 0));
        GameObject.Find("DoorRight").SetActive( GridManager.Instance.IsOpen(roomRow, roomCol, 1));
        GameObject.Find("DoorBottom").SetActive(GridManager.Instance.IsOpen(roomRow, roomCol, 2));
        GameObject.Find("DoorLeft").SetActive(  GridManager.Instance.IsOpen(roomRow, roomCol, 3));

        FindFirstObjectByType<Minimap>()?.Draw(); // redraw minimap with current room highlighted
    }

    private void CheckIfCleared(int roomRow, int roomCol)
    {
        // Only load enemies and collectibles if room hasn't been cleared yet
        if(GridManager.Instance.clearedRooms[roomRow, roomCol])
        {
            // Dont load the enemies
            GameObject roomEnemies = GameObject.Find("Enemies");
            roomEnemies?.SetActive(false);

            // Dont load collected upgrades
            GameObject rangedAttackUpgrade = GameObject.Find("RangedAttackUpgrade");
            rangedAttackUpgrade?.SetActive(false);
        }
    }
}