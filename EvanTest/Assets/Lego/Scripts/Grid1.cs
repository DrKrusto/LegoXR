using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class Grid1 : MonoBehaviour {
		
		public Vector2 gridSize = new Vector2 (1, 1);

		Vector2 gridOffset;
		Vector3 lastGridSize;
	    Vector3 lastPosition;

		public List<Vector4> occupiedPositions;

		
		// Update transform and texture size
		// Actualizar escala del objeto y su textura
		void UpdateScale () {
				transform.localScale = new Vector3 (gridSize.x, gridSize.y, 1);
				GetComponent<Renderer> ().material.mainTextureScale = gridSize;
		}

		// Obtener el Offset de la cuadricula
		public Vector2 GetGridOffset () {
				gridOffset.x = transform.localPosition.x;
				gridOffset.y = transform.localPosition.y;
				return gridOffset;
		}
}
