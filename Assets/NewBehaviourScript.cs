using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter))]
public class NewBehaviourScript : MonoBehaviour
{
	Mesh mesh;
	Mesh mesh2;
	Mesh meshLeftUP;
	Mesh meshLeftDown;
	Mesh meshLeft;
	Mesh meshRight;
	Mesh meshRightUp;
	Mesh meshRightDown;
	Vector3[] vectors;
	Vector3[] vectors2;
	GameObject[] gameObjs;
	int meshIndex;


	Mesh point;
	Vector3[] pointMeshVectors;
	int[] pointMeshTriangles;

	GameObject obj;
	GameObject obj2;
	GameObject leftUp;
	GameObject leftDown;
	GameObject left;
	GameObject right;
	GameObject rightUp;
	GameObject rightDown;

	GameObject flashPoint;

	int startTime=0;
	int time = 10;



	Material newMat;
	Material oldMat;

	Vector3[] frontMeshVectors;
	int[] frontMeshTriangles;

	Vector3[] backMeshVectors;
	int[] backMeshTriangles;

	Vector3[] leftUpMeshVectors;
	int[] leftUpMeshTriangles;

	Vector3[] leftDownMeshVectors;
	int[] leftDownMeshTriangles;


	Vector3[] leftMeshVectors;
	int[] leftMeshTriangles;

	Vector3[] rightMeshVectors;
	int[] rightMeshTriangles;

	Vector3[] rightUpMeshVectors;
	int[] rightUpMeshTriangles;

	Vector3[] rightDownMeshVectors;
	int[] rightDownMeshTriangles;

	int[] triangles;
	int[] triangles2;

	int center_x = 0;
	int center_y = 0;
	int center_z = 0;
	int radius = 2;
	int pointNum = 30;

	Vector3 circleCenter;

	void Awake ()
	{

		obj = GameObject.Find ("MeshObject");
		obj2 = GameObject.Find ("MeshObject2");
		leftUp = GameObject.Find ("MeshObjectLeftUp");
		leftDown = GameObject.Find ("MeshObjectLeftDown");
		left = GameObject.Find ("MeshObjectLeft");
		right = GameObject.Find ("MeshObjectRight");
		rightUp = GameObject.Find ("MeshObjectRightUp");
		rightDown = GameObject.Find ("MeshObjectRightDown");
		flashPoint = GameObject.Find ("PointObject");

		mesh = obj.GetComponent<MeshFilter> ().mesh;
		mesh2 = obj2.GetComponent<MeshFilter> ().mesh;
		meshLeftUP = leftUp.GetComponent<MeshFilter> ().mesh;
		meshLeftDown = leftDown.GetComponent<MeshFilter> ().mesh;
		meshLeft = left.GetComponent<MeshFilter> ().mesh;
		meshRight = right.GetComponent<MeshFilter> ().mesh;
		meshRightUp = rightUp.GetComponent<MeshFilter> ().mesh;
		meshRightDown = rightDown.GetComponent<MeshFilter> ().mesh;
		point = flashPoint.GetComponent<MeshFilter> ().mesh;
	}




	void Start ()
	{
		CreateCircleCenter ();
//		MakeFrontMesh ();
//		MakefixedMesh ();
//		MakeMeshData ();

		oldMat = Resources.Load("New Material", typeof(Material)) as Material;
		newMat = Resources.Load("New Material 2", typeof(Material)) as Material;

		MakeFontOrBackMesh (true);
		MakeFontOrBackMesh (false);
		MakeLeftUpOrLeftDownMesh (true);
		MakeLeftUpOrLeftDownMesh (false);
		MakeLeftOrRightMesh (true);
		MakeLeftOrRightMesh (false);
		MakeRightUpOrRightDownMesh (true);
		MakeRightUpOrRightDownMesh (false);
		organizeMeshs ();
		MakeFlashingPoint (0.3f, 0.3f, 0.05f);

		CreateMesh ();


	}



	void CreateCircleCenter ()
	{

		circleCenter = new Vector3 (center_x, center_y, center_z);
	}


	void MakeFontOrBackMesh (bool isFront)
	{
		Vector3[] meshVectors;
		int[] meshTriangles;
	
		float initDegree = 69f;
		float initRad = (float)(initDegree * Mathf.PI / 180f);
		meshVectors = new Vector3[pointNum];
		float length = 2f * Mathf.Cos (initRad) * radius;
		float step = length / (pointNum - 2);
		for (int i = 0; i < pointNum - 1; i++) {
			//			float rad = (initDegree + i * 45 / (pointNum - 2)) * Mathf.PI / 360;
			//			frontMeshVectors [i] = new Vector3 (centre_x - radius * Mathf.Cos (rad), centre_y + radius * Mathf.Sin (rad), 0);

			float x = center_x - radius * Mathf.Cos (initRad) + i * step;
			float y;
			if (isFront) {
				y = Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2)) + center_y;
			} else {
				y = center_y - Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2));
			}
			meshVectors [i] = new Vector3 (x, y, center_z);
		}

		meshVectors [pointNum - 1] = new Vector3 (center_x, center_y, center_z);


		//		frontMeshVectors = new Vector3[]{new Vector3(0,0,0), new Vector3(0,2,0), new Vector3(1,0,0)};
		//
		//
		//		frontMeshTriangles = new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		meshTriangles = new int[(pointNum - 2) * 3];

		for (int i = 0; i < pointNum - 2; i++) {
			for (int j = 0; j < 3; j++) {
				if (j == 0) {
					meshTriangles [i * 3 + j] = i;
				} else if (j == 1) {
					meshTriangles [i * 3 + j] = i + 1;
				} else if (j == 2) {
					meshTriangles [i * 3 + j] = pointNum - 1;
				}

			}
		}

		if (isFront) {
			frontMeshVectors = meshVectors;
			frontMeshTriangles = meshTriangles;
		} else {
			backMeshVectors = meshVectors;
			backMeshTriangles = meshTriangles;
		}
	}


	void MakeLeftUpOrLeftDownMesh (bool isLeftUp)
	{

		Vector3[] meshVectors;
		int[] meshTriangles;

		float initDegree = 24f;
		float initRad = (float)(initDegree * Mathf.PI / 180f);
		float endDegree = 66f;
		float endRad = (float)(endDegree * Mathf.PI / 180f);
		meshVectors = new Vector3[pointNum];
		float length = (Mathf.Cos (initRad) - Mathf.Cos (endRad)) * radius;
		float step = length / (pointNum - 2);
		for (int i = 0; i < pointNum - 1; i++) {
			//			float rad = (initDegree + i * 45 / (pointNum - 2)) * Mathf.PI / 360;
			//			frontMeshVectors [i] = new Vector3 (centre_x - radius * Mathf.Cos (rad), centre_y + radius * Mathf.Sin (rad), 0);

			float x = center_x - radius * Mathf.Cos (initRad) + i * step;
			float y;
			if (isLeftUp) {
				y = Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2)) + center_y;
			} else {
				y = center_y - Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2));
			}
			meshVectors [i] = new Vector3 (x, y, center_z);
		}

		meshVectors [pointNum - 1] = new Vector3 (center_x, center_y, center_z);


		//		frontMeshVectors = new Vector3[]{new Vector3(0,0,0), new Vector3(0,2,0), new Vector3(1,0,0)};
		//
		//
		//		frontMeshTriangles = new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
		meshTriangles = new int[(pointNum - 2) * 3];

		for (int i = 0; i < pointNum - 2; i++) {
			for (int j = 0; j < 3; j++) {
				if (j == 0) {
					meshTriangles [i * 3 + j] = i;
				} else if (j == 1) {
					meshTriangles [i * 3 + j] = i + 1;
				} else if (j == 2) {
					meshTriangles [i * 3 + j] = pointNum - 1;
				}

			}
		}

		if (isLeftUp) {
			leftUpMeshVectors = meshVectors;
			leftUpMeshTriangles = meshTriangles;
		} else {
			leftDownMeshVectors = meshVectors;
			leftDownMeshTriangles = meshTriangles;
		}
	}


	void MakeLeftOrRightMesh (bool isLeft)
	{
		
		Vector3[] meshVectors;
		int[] meshTriangles;
		float initDegree = 21f;
		float initRad = (float)(initDegree * Mathf.PI / 180f);

		meshVectors = new Vector3[pointNum];
		float length = 2 * radius * Mathf.Sin (initRad);
		float step = length / (pointNum - 2);



		for (int i = 0; i < pointNum - 1; i++) {
			//			float rad = (initDegree + i * 45 / (pointNum - 2)) * Mathf.PI / 360;
			//			frontMeshVectors [i] = new Vector3 (centre_x - radius * Mathf.Cos (rad), centre_y + radius * Mathf.Sin (rad), 0);

			float y = center_y - radius * Mathf.Sin (initRad) + i * step;
			float x;
			if (isLeft) {
				x = -Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (y - center_y, 2)) + center_x;
			} else {
				x = center_x + Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (y - center_y, 2));
			}
			meshVectors [i] = new Vector3 (x, y, center_z);
		}


		meshVectors [pointNum - 1] = new Vector3 (center_x, center_y, center_z);

		meshTriangles = new int[(pointNum - 2) * 3];

		for (int i = 0; i < pointNum - 2; i++) {
			for (int j = 0; j < 3; j++) {
				if (j == 0) {
					meshTriangles [i * 3 + j] = i;
				} else if (j == 1) {
					meshTriangles [i * 3 + j] = i + 1;
				} else if (j == 2) {
					meshTriangles [i * 3 + j] = pointNum - 1;
				}

			}
		}

		if (isLeft) {
			leftMeshVectors = meshVectors;
			leftMeshTriangles = meshTriangles;
		} else {
			rightMeshVectors = meshVectors;
			rightMeshTriangles = meshTriangles;
		}

	}

	void MakeRightUpOrRightDownMesh (bool isUp)
	{
		
		Vector3[] meshVectors;
		int[] meshTriangles;
		float initDegree = 66f;
		float initRad = (float)(initDegree * Mathf.PI / 180f);
		float endDegree = 24f;
		float endRad = (float)(endDegree * Mathf.PI / 180f);

		meshVectors = new Vector3[pointNum];
		float length = radius * Mathf.Cos (endRad) - radius * Mathf.Cos (initRad);
		float step = length / (pointNum - 2);


		for (int i = 0; i < pointNum - 1; i++) {
			float x = center_x + radius * Mathf.Cos (initRad) + i * step;
			float y;
			if (isUp) {
				y = Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2)) + center_y;
			} else {
				y = center_y - Mathf.Sqrt (Mathf.Pow (radius, 2) - Mathf.Pow (x - center_x, 2));
			}
			meshVectors [i] = new Vector3 (x, y, center_z);
		}

		meshVectors [pointNum - 1] = new Vector3 (center_x, center_y, center_z);

		meshTriangles = new int[(pointNum - 2) * 3];

		for (int i = 0; i < pointNum - 2; i++) {
			for (int j = 0; j < 3; j++) {
				if (j == 0) {
					meshTriangles [i * 3 + j] = i;
				} else if (j == 1) {
					meshTriangles [i * 3 + j] = i + 1;
				} else if (j == 2) {
					meshTriangles [i * 3 + j] = pointNum - 1;
				}

			}
		}

		if (isUp) {
			rightUpMeshVectors = meshVectors;
			rightUpMeshTriangles = meshTriangles;
		} else {
			rightDownMeshVectors = meshVectors;
			rightDownMeshTriangles = meshTriangles;
		}
	}

	void CreateMesh ()
	{
		//		mesh.Clear ();
		//		mesh.vertices = vectors;
		//		mesh.triangles = triangles;
		//		mesh2.Clear ();
		//		mesh2.vertices = vectors2;
		//		mesh2.triangles = triangles2;

		mesh.Clear ();
		mesh.vertices = frontMeshVectors;
		mesh.triangles = frontMeshTriangles;

		mesh2.Clear ();
		mesh2.vertices = backMeshVectors;
		mesh2.triangles = backMeshTriangles;

		meshLeftUP.Clear ();
		meshLeftUP.vertices = leftUpMeshVectors;
		meshLeftUP.triangles = leftUpMeshTriangles;

		meshLeftDown.Clear ();
		meshLeftDown.vertices = leftDownMeshVectors;
		meshLeftDown.triangles = leftDownMeshTriangles;

		meshLeft.Clear ();
		meshLeft.vertices = leftMeshVectors;
		meshLeft.triangles = leftMeshTriangles;

		meshRight.Clear ();
		meshRight.vertices = rightMeshVectors;
		meshRight.triangles = rightMeshTriangles;

		meshRightUp.Clear ();
		meshRightUp.vertices = rightUpMeshVectors;
		meshRightUp.triangles = rightUpMeshTriangles;

		meshRightDown.Clear ();
		meshRightDown.vertices = rightDownMeshVectors;
		meshRightDown.triangles = rightDownMeshTriangles;

		point.Clear ();
		point.vertices = pointMeshVectors;
		point.triangles = pointMeshTriangles;
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("您按下了A键");
			showNextMesh ();
		} 

		if (startTime < time) {
//			transform.renderer.matirial=mt1;
			flashPoint.GetComponent<Renderer> ().material = oldMat;
			startTime++;
		} else if (startTime >= time && startTime < 2 * time) {
//			transform.renderer.matirial=mt2;
			flashPoint.GetComponent<Renderer> ().material = newMat;
			startTime++;
		} else {
			startTime = 0;
		}

	}

	void showNextMesh(){

		int currentMesh = meshIndex;
		if (meshIndex == 7) {
			meshIndex = 0;

		} else {
			meshIndex++;
		}
		gameObjs [meshIndex].GetComponent<Renderer> ().material = newMat;
		gameObjs [currentMesh].GetComponent<Renderer> ().material = oldMat;

	}

	void organizeMeshs(){
		gameObjs = new GameObject[8];
		gameObjs[0] = obj;
		gameObjs[1] = leftUp;
		gameObjs[2] = left;
		gameObjs[3] = leftDown;
		gameObjs[4] = obj2;
		gameObjs[5] = rightDown;
		gameObjs[6] = right;
		gameObjs[7] = rightUp;


//		GameObject obj = GameObject.Find ("MeshObject");
//		GameObject obj2 = GameObject.Find ("MeshObject2");
//		GameObject leftUp = GameObject.Find ("MeshObjectLeftUp");
//		GameObject leftDown = GameObject.Find ("MeshObjectLeftDown");
//		GameObject left = GameObject.Find ("MeshObjectLeft");
//		GameObject right = GameObject.Find ("MeshObjectRight");
//		GameObject rightUp = GameObject.Find ("MeshObjectRightUp");
//		GameObject rightDown = GameObject.Find ("MeshObjectRightDown");
		
	}

	void MakeFlashingPoint(float x, float y, float r){

		pointMeshVectors = new Vector3[]{ new Vector3(x-r, y-r, center_z), new Vector3(x-r, y+r,center_z), new Vector3(x+r,y+r, center_z), new Vector3(x+r, y-r,center_z)};
		pointMeshTriangles = new int[]{0,1,3,1,2,3};

	}
}
