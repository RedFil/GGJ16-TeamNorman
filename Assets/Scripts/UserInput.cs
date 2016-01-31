﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : MonoBehaviour {
	public float inputHorizontal = 0f;
	public float inputVertical = 0f;
	public bool inputMouseButton0 = false;

	public GameObject mainCharacter;
	private PlayerBehaviour playerBehave;

//	private List<Tile> movePath = new List<Tile> ();
//	private int curPathIndex = 0;
	
	public TileManager tileManager;
	public PlayerStatus playerStatus;
	public MapLoader mapLoader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inputHorizontal = Input.GetAxisRaw ("Horizontal");
		inputVertical = Input.GetAxisRaw ("Vertical");
		inputMouseButton0 = Input.GetMouseButton (0);

		// Move the player, if possible
		if (playerBehave != null && !playerBehave.isMoving) {
//			if (curPathIndex < movePath.Count-1) {
//				
//				Direction dir = ;
//
//				// Start the move animation
//				playerBehave.MoveDirection (dir);
//				LoadMapOnWalk ();
//			} else 
			Vector2i newPosition = playerStatus.playerGridPosition;
			if (inputHorizontal != 0) {
				Direction dir = inputHorizontal > 0 ? Direction.Right : Direction.Left;
				newPosition.x += (int)inputHorizontal;


				if (tryToMoveTo (newPosition)) {
					// Start the move animation
					playerBehave.MoveDirection (dir);
					LoadMapOnWalk ();
				}
			} else if (inputVertical != 0) {
				Direction dir = inputVertical > 0 ? Direction.Up : Direction.Down;
				newPosition.y += (int)inputVertical;

				if (tryToMoveTo (newPosition)) {
					// Start the move animation
					playerBehave.MoveDirection (dir);
					LoadMapOnWalk ();
				}
			}
//			else if (inputMouseButton0) {
//				GetMouseClickPosition ();
//			}
		}
	}

	void OnLevelWasLoaded (int level) {
		// When the DayScene(1) is loaded, get the MainCharacter
		if (level == 1) {
			mainCharacter = GameObject.Find ("MainCharacter");

			if (mainCharacter == null) {
				Debug.LogError ("No 'MainCharacter' was found in the scene " + level + ".");
			} else {
				playerBehave = mainCharacter.GetComponent<PlayerBehaviour> ();

				if (playerBehave == null) {
					Debug.LogError ("No PlayerBehaviour script was found in the 'mainCharacter'.");
				}
			}
		}
	}

	void LoadMapOnWalk () {
		mapLoader.LoadMapForWalk ();
		mapLoader.DisablePlayerGridTile ();
	}

//	void GetMouseClickPosition () {
//		// Converting Mouse Pos to 2D (Vector2) World Pos
//		Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
//		RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);
//
//		if (hit) {
//			Vector2i hitPos = new Vector2i ((int)hit.transform.position.x, (int)hit.transform.position.y);
//
//			// Convert 3D position into Grid Position
//			hitPos.x /= (int)hit.collider.gameObject.GetComponent<SpriteRenderer> ().bounds.size.x;
//			hitPos.y /= (int)hit.collider.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
//
//			// Pathfinder
//			movePath = tileManager.getPath (playerStatus.playerGridPosition, hitPos, playerStatus.playerEnergy);
//			curPathIndex = 0;
//		}
//	}

	bool tryToMoveTo (Vector2i newPosition) {
		Tile currTile = tileManager.getTile (newPosition);
		float deathBonus = 1;
		if (playerStatus.bonuses[2]) {
			deathBonus = 2f / 3f;
		}
		int cost;
		switch (currTile.Type) {
		case Tile.TileType.Plains:
			cost = Mathf.RoundToInt (Tile.defaultTerrainPenalties [(int)Tile.TileType.Plains] * deathBonus);
			if (playerStatus.playerEnergy >= cost) {
				playerStatus.playerEnergy -= cost;
				playerStatus.playerGridPosition = newPosition;
				return true;
			}
			return false;
		case Tile.TileType.Mountain:
			cost = Mathf.RoundToInt (Tile.defaultTerrainPenalties [(int)Tile.TileType.Mountain] * deathBonus);
			if (playerStatus.playerEnergy >= cost) {
				playerStatus.playerEnergy -= cost;
				playerStatus.playerGridPosition = newPosition;
				return true;
			}
			return false;
		case Tile.TileType.Cave:
			cost = Mathf.RoundToInt (Tile.defaultTerrainPenalties [(int)Tile.TileType.Cave] * deathBonus);
			if (playerStatus.playerEnergy >= cost) {
				playerStatus.playerEnergy -= cost;
				playerStatus.playerGridPosition = newPosition;
				return true;
			}
			return false;
		case Tile.TileType.Graveyard:
			cost = Mathf.RoundToInt (Tile.defaultTerrainPenalties [(int)Tile.TileType.Graveyard] * deathBonus);
			if (playerStatus.playerEnergy >= cost) {
				playerStatus.playerEnergy -= cost;
				playerStatus.playerGridPosition = newPosition;
				return true;
			}
			return false;
		case Tile.TileType.Volcano:
			cost = Mathf.RoundToInt (Tile.defaultTerrainPenalties [(int)Tile.TileType.Volcano] * deathBonus);
			if (playerStatus.playerEnergy >= cost) {
				playerStatus.playerEnergy -= cost;
				playerStatus.playerGridPosition = newPosition;
				return true;
			}
			return false;
		default:
			return false;
		}
	}
}
 