using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    public GameObject quickStartButton;
    public GameObject quickCancelButton;
    public int roomSize;
    
    
    /// <summary>
    /// When connected to a server synchronises the scene with the others players in the game (to change if we decide that players of the same game can be in different scenes
    /// And shows the Start button (given in parameter via Unity
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    /// <summary>
    /// When the button is pressed :
    ///     - Disables the start button
    ///     - Enables the cancel button
    ///     - Joins a random room
    /// </summary>
    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    /// <summary>
    /// If connecting didn't work (no free game), tries to create another
    /// </summary>
    /// <param name="returnCode"> Useless parameter</param>
    /// <param name="message"> Useless parameter</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRandomRoom();
    }

    
    /// <summary>
    /// Creates a room containing roomSize players (given via Unity as 2) in a room named from Room0 to Room9999
    /// here for random, for private, 2 or 3 little things to change (later in the project)
    /// </summary>
    void CreateRandomRoom()
    {
        Debug.Log("Creating a new room");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte) roomSize};
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
        Debug.Log("Room" + randomRoomNumber);
    }

    
    /// <summary>
    /// If the number of the Room is already taken, tries again
    /// </summary>
    /// <param name="returnCode"> Useless parameter</param>
    /// <param name="message"> Useless parameter</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room, trying again");
        CreateRandomRoom();
    }

    
    /// <summary>
    /// Opposite of QuickStart :
    ///     - Disables Cancel  button
    ///     - Enables Start Button
    ///     - Leaves the room
    /// </summary>
    public void QuickCancel()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        Debug.Log("Quick cancel");
        PhotonNetwork.LeaveRoom();
    }
}
