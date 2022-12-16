// Created by Carlos Arturo Rodriguez Silva https://www.youtube.com/channel/UCUhamcnct2QpDcNtfwycl1g/videos or https://www.facebook.com/legendxh

using UnityEngine;
using System.Collections;

public class DragAndDropLego1 : MonoBehaviour {

	[Header ("Restrictions")]
	public bool considerScale = true;
	public bool considerOtherObjects = true;

	[Space(5)]

	public Vector2 dragScale = new Vector2 (1, 1);
	public Vector4 currentPosition = new Vector4 (1, 1, 1, 1);

	Vector2 gridOffset = Vector2.zero;
	Vector2 gridSize = Vector2.one;
	Vector3 screenPoint;

	Vector4 lastPos;
	Vector3 lastParentPos;

	Vector4 targetPos;


	// Get recent values of the Grid
	// Obtener los valores mas recientes del Grid
	void UpdateGridData () {
		gridSize = FindObjectOfType<Grid> ().gridSize;
		gridOffset = FindObjectOfType<Grid> ().GetGridOffset ();	
	}

	public void UpdateAll () {
		UpdatePosition ();
		AddPosition (lastPos);
	}


	// Agregar posicion
	void AddPosition (Vector4 pos) {
		var grid = FindObjectOfType<Grid> ();
		if (!grid.occupiedPositions.Contains (pos)) {
			grid.occupiedPositions.Add (pos);
			// Debug.Log ("Added: " + pos);
		}
	}

	// Eliminar posicion
	void RemovePosition (Vector4 pos) {
		var grid = FindObjectOfType<Grid> ();
		if (grid.occupiedPositions.Contains (pos)) {
			grid.occupiedPositions.Remove (pos);
			// Debug.Log ("Removed: " + pos);
		}
	}

	// Update object position variable
	// Actualizar la variable de la posicion del objeto
	void UpdatePosition () {
		currentPosition.x = transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f;
		currentPosition.y = (transform.parent.position.x + (gridSize.x * 0.5f) + 0.5f) + transform.localScale.x - 1;

		currentPosition.z = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f);
		currentPosition.w = -(transform.parent.position.y - (gridSize.y * 0.5f) - 0.5f) + transform.localScale.y - 1;

		// Save actual position
		// Guardar posicion actual
		lastParentPos = transform.parent.position;
		lastPos = currentPosition;
	}

	// Function that allows you to move an object according to the Grid
	// Funcion que permite mover un objeto segun la Cuadricula
	Vector3 SnapToGrid(Vector3 dragPos)
	{
		// If X is even, fix the target position
		// Si es X es par, corregir la posicion de destino
		if (gridSize.x % 2 == 0) {
			dragPos.x = (Mathf.Round( dragPos.x / dragScale.x ) * dragScale.x) + 0.5f;
		} else {
			dragPos.x = (Mathf.Round (dragPos.x / dragScale.x) * dragScale.x);
		}

		// If Y is even, fix the target position
		// Si es Y es par corregir la posicion de destino
		if (gridSize.y % 2 == 0) {
			dragPos.y = (Mathf.Round( dragPos.y / dragScale.y ) * dragScale.y) + 0.5f;
		} else {
			dragPos.y = (Mathf.Round (dragPos.y / dragScale.y) * dragScale.y);
		}

		#region Restrictions

		// Restrict exit from grid
		// Restringir que se pueda salir de la cuadricula
		var maxXPos = ((gridSize.x - 1) * 0.5f) + gridOffset.x;
		var maxYPos = ((gridSize.y - 1) * 0.5f) + gridOffset.y;

		// Considering GameObject Scale
		// Considerando la escala del objeto
		if (considerScale) {

			if (dragPos.x > maxXPos - transform.localScale.x + 1) {
				dragPos.x = maxXPos - transform.localScale.x + 1;
			}

			if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x) {
				dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
			}

			if (dragPos.y > maxYPos) {
				dragPos.y = maxYPos;
			}

			if (dragPos.y < (-maxYPos + gridOffset.y + gridOffset.y) + transform.localScale.y - 1) {
				dragPos.y = -maxYPos + gridOffset.y + gridOffset.y + transform.localScale.y - 1;
			}
		}

		else
		{

			if (dragPos.x > maxXPos)  {
				dragPos.x = maxXPos;
			}

			if (dragPos.x < -maxXPos + gridOffset.x + gridOffset.x) {
				dragPos.x = -maxXPos + gridOffset.x + gridOffset.x;
			}

			if (dragPos.y > maxYPos) {
				dragPos.y = maxYPos;
			}

			if (dragPos.y < -maxYPos + gridOffset.y + gridOffset.y) {
				dragPos.y = -maxYPos + gridOffset.y + gridOffset.y;
			}
		}

		#endregion

		return dragPos;
	}
}



