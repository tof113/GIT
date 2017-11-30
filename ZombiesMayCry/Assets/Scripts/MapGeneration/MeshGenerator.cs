using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator : MonoBehaviour {


	List <Vector3> vertices;//liste des positions
	List <int> triangles;
	public MeshFilter cave;
	private MeshCollider wallCollider = null;

	public MeshFilter walls;
	public float wallHeight = 5;

	public SquareGrid squareGrid;
	//savoir a quel triangle un vertex appartient =>key = int Vertex index -> tous les triangles qui sont composé de ce vertex
	Dictionary<int, List<Triangle>> triangleDictionnary = new Dictionary<int, List<Triangle>>() ;
	//enregistre la list des limites (vertex par vertex)
	List<List<int>> outlines = new List<List<int>>();
	//evite de vérifier le même vertex plusieurs fois!! -> performence (contains)
	HashSet<int> checkedVertecies = new HashSet<int> ();

	/*
	 * Genere les mash de la map
	 */ 
	public void GenerateMesh(int[,] map, float squareSize){
		outlines.Clear ();
		checkedVertecies.Clear ();
		triangleDictionnary.Clear ();
		squareGrid = new SquareGrid (map, squareSize);

		vertices = new List<Vector3> ();
		triangles = new List<int> ();

		for (int x = 0; x < squareGrid.squares.GetLength (0); x++) {
			for (int y = 0; y < squareGrid.squares.GetLength (1); y++) {
				TriangulateSquare (squareGrid.squares [x, y]);
			}
		}

		Mesh mesh = new Mesh ();
		cave.mesh = mesh;

		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals (); 

		//texture
		int tileAmount =10;
		Vector2[] uvs = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; i++) {
			float percentX = Mathf.InverseLerp (-map.GetLength (0) / 2 * squareSize, map.GetLength(0)/2*squareSize, vertices [i].x) *tileAmount;
			float percentY = Mathf.InverseLerp (-map.GetLength (1) / 2 * squareSize, map.GetLength(0)/2*squareSize, vertices [i].z) * tileAmount;
			uvs [i] = new Vector2 (percentX, percentY);
		}
		mesh.uv = uvs;

		RemoveWallMesh ();
		CreateWallMesh ();
	}
	void RemoveWallMesh(){
		if (wallCollider) {
			Destroy (wallCollider.GetComponent<Rigidbody> ());
			Destroy (wallCollider);

		}

	}
	/*
	 * va suivre les limites trouvées générer les murs
	 */ 
	void CreateWallMesh(){
		CalculateMeshOutlines ();

		List<Vector3> wallVertecies = new List<Vector3> ();
		List<int> wallTriangles = new List<int> ();
		Mesh wallMesh = new Mesh ();

		//parcours les limites
		foreach (List<int> outline in outlines) {
			//parcours tous les vertex de la limite
			for (int i = 0; i < outline.Count - 1; i++) {
				int startIndex = wallVertecies.Count;
				// creer le "pan du mur" 
				wallVertecies.Add (vertices [outline [i]]);//left = 0
				wallVertecies.Add (vertices [outline [i + 1]]);//right = 1
				wallVertecies.Add (vertices [outline [i]] - Vector3.up * wallHeight);//bottom left =2
				wallVertecies.Add (vertices [outline [i + 1]] - Vector3.up * wallHeight);//bottom right =3
			
				//creer les 2 triangles qui compose ce pan du mur
				wallTriangles.Add (startIndex + 0);
				wallTriangles.Add (startIndex + 2);
				wallTriangles.Add (startIndex + 3);

				wallTriangles.Add (startIndex + 3);
				wallTriangles.Add (startIndex + 1);
				wallTriangles.Add (startIndex + 0);

			}
		}

		wallMesh.vertices = wallVertecies.ToArray ();
		wallMesh.triangles = wallTriangles.ToArray ();

		walls.mesh = wallMesh;

		wallCollider = walls.gameObject.AddComponent<MeshCollider> ();
		//wallCollider.GetComponent<MeshCollider> ().convex = true;
		//wallCollider.GetComponent<MeshCollider> ().isTrigger = true;
		wallCollider.sharedMesh = wallMesh;
		//wallCollider.gameObject.AddComponent<Rigidbody> ();
		//wallCollider.GetComponent<Rigidbody> ().useGravity = false;
		//wallCollider.GetComponent<Rigidbody> ().isKinematic = true;

	}

	/*
	 * selon la configuration, trouve les points qui doivent être reliés pour faire le bon mesh
	 */ 
	void TriangulateSquare(Square square){
		switch (square.configuration) {
		case 0:
			break;

			//1
		case 1:
			MeshFromPoints (square.centerLeft, square.centerBottom, square.bottomLeft);
			break;
		case 2: 
			MeshFromPoints (square.bottomRight, square.centerBottom, square.centerRight);
			break;
		case 4: 
			MeshFromPoints (square.topRight, square.centerRight, square.centerTop);
			break;
		case 8 : 
			MeshFromPoints (square.topLeft, square.centerTop, square.centerLeft);
			break;

			//2 points
		case 3:
			MeshFromPoints (square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 6:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
			break;
		case 9:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerLeft);
			break;
		case 5:
			MeshFromPoints (square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
			break;
		case 10:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

			//3 points

		case 7:
			MeshFromPoints (square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
			break;
		case 11:
			MeshFromPoints (square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints (square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
			break;

			//4points => si tous les points sont actif => tout les coté sont des murs => donc acun ne peut être une limite ! => aoté dans le check pour ne jamais les parcourir !
		case 15:
			MeshFromPoints (square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVertecies.Add (square.topLeft.vertexIndex);
			checkedVertecies.Add (square.topRight.vertexIndex);
			checkedVertecies.Add (square.bottomLeft.vertexIndex);
			checkedVertecies.Add (square.bottomRight.vertexIndex);

			break;
		}
	}
	/*
	 * prend un certains nombres de points et crée un mesh avec ces points.
	 */ 
	void MeshFromPoints(params Node[] points){
		AssignVertecies (points);

		if (points.Length >= 3) {
			CreateTriangle (points [0], points [1], points [2]);
		}
		if (points.Length >= 4) {
			CreateTriangle (points [0], points [2], points [3]);
		}
		if (points.Length >= 5) {
			CreateTriangle (points [0], points [3], points [4]);
		}
		if (points.Length >= 6) {
			CreateTriangle (points [0], points [4], points [5]);
		}
	}
	/*
	 * les points doivent être assigné à la liste des vertices
	 */ 
	void AssignVertecies(Node[] points){
		for (int i = 0; i < points.Length; i++) {
			if (points [i].vertexIndex == -1) {// point not assigned yet
				points[i].vertexIndex = vertices.Count;
				vertices.Add (points [i].position);
			}
		}
	}
	/*
	 * créer les triangles pour les meshs (triangle composé de trois vertex)
	 * et ajoute ce triangle dans le dico
	 */ 
	void CreateTriangle(Node a, Node b , Node c){
		triangles.Add (a.vertexIndex);
		triangles.Add (b.vertexIndex);
		triangles.Add (c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriangleToDictionnary (triangle.vertexIndexA, triangle);
		AddTriangleToDictionnary (triangle.vertexIndexB, triangle);
		AddTriangleToDictionnary (triangle.vertexIndexC, triangle);

	}
	/**
	 * ajoute le triangle dans la liste des triangles avec ce vertex dans le dictionnaire
	 */ 
	void AddTriangleToDictionnary(int vertexIndexKey, Triangle triangle){
		if(triangleDictionnary.ContainsKey(vertexIndexKey)){
			triangleDictionnary[vertexIndexKey].Add(triangle);
		}else{
			List<Triangle> triangleList = new List<Triangle> ();
			triangleList.Add (triangle);
			triangleDictionnary.Add (vertexIndexKey, triangleList);
		}
	}
	/*
	 * parcours tous les vertex de la map et vériffie si c'est une limite, et va suivre la limite jusqu'a ce que ca "boucle" puis ajoute a la liste
	 */ 
	void CalculateMeshOutlines(){
		for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++) {
			if (!checkedVertecies.Contains (vertexIndex)) {
				int newOutlineVertex = GetConnectedOutlineVertex (vertexIndex);
				if (newOutlineVertex != -1) {
					checkedVertecies.Add (vertexIndex);

					List<int> newOutline = new List<int> ();
					newOutline.Add (vertexIndex);
					outlines.Add (newOutline);
					FollowOutlines (newOutlineVertex, outlines.Count - 1);//outlines.Count -1 => la limite que vient d'etre ajoutée
					outlines [outlines.Count - 1].Add (vertexIndex);
				}
			}
		}

	}
	/*
	 * permet de trouver, avec un vertex qui est sur la limite, un autre vertex qui est sur la limite 
	 */ 
	int GetConnectedOutlineVertex(int vertexIndex){
		//triangles contenant le vertex
		List<Triangle> trianglesContaingVertex = triangleDictionnary [vertexIndex];

		for (int i = 0; i < trianglesContaingVertex.Count; i++) {
			Triangle triangle = trianglesContaingVertex [i];
			//parcours tous les vertex pour voir si ils forment une limite avec le vertex de base
			for (int j = 0; j < 3; j++) {
				int vertexB = triangle [j];
				//evite que vertexB soit == a vertexIndex 
				if (vertexB != vertexIndex && !checkedVertecies.Contains(vertexB)) {
					if (IsOutlineEdge (vertexIndex, vertexB)) {
						return vertexB;
					}
				}
			}
		}
		return -1;
	}

	/*
	 * tant qu'on touve un vertex qui est une limite, la méthode recommence avec le nouveau vertex limite dea trouvé
	 */ 
	void FollowOutlines(int vertexIndex, int outlineIndex){
		outlines [outlineIndex].Add (vertexIndex);
		checkedVertecies.Add (vertexIndex);
		int nextVertexIndex = GetConnectedOutlineVertex (vertexIndex);

		if (nextVertexIndex != -1) {
			FollowOutlines (nextVertexIndex, outlineIndex);
		}
	}
	/*
	 * determine si le bord déterminé par les 2 vertex sest une limite => ont exactement 1 triangle en commun
	 */ 
	bool IsOutlineEdge(int vertexA, int vertexB){
		List<Triangle> trianglesContainingVertexA = triangleDictionnary [vertexA];
		int sharedTriangleCount = 0;
		for (int i = 0; i < trianglesContainingVertexA.Count; i++) {
			if (trianglesContainingVertexA [i].Contains (vertexB)) {
				sharedTriangleCount++;
				if (sharedTriangleCount > 1) {
					break;
				}
			}
		}
		return sharedTriangleCount == 1;
	}

	/*
	 * triangle qui contient les 3 vertex du triangle
	 * 
	 */ 
	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int [] vertecies;

		public Triangle(int a, int b, int c){
			vertexIndexA =a;
			vertexIndexB =b;
			vertexIndexC = c;

			vertecies = new int[3];
			vertecies[0] = a;
			vertecies[1] = b;
			vertecies[2] = c;

		}
		//permet d'acceder au vertex du triangles comme un tableau
		public int this[int i]{
			get{
				return vertecies [i];
			}
		}
		//determine si un vertex est dans le triangle ou pas
		public bool Contains(int vertexIndex){
			return (vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC);
		}
	}


	/*
	 * contient le tableau qui retient les carrés
	 * calcule la position de chaque controleNode
	 */ 
	public class SquareGrid{
		public Square [,] squares;

		public SquareGrid(int [,] map, float squareSize){
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);

			float mapWidth = nodeCountX * squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for(int x = 0 ; x < nodeCountX; x++){//parcour la map et crée les ControleNode
				for(int y = 0; y < nodeCountY ; y++){
					Vector3 pos = new Vector3(-mapWidth/2 +x * squareSize +  squareSize/2, 0 , -mapHeight/2 +y * squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos, map[x,y] == 1, squareSize);
				}
			}
			//creer la map de carrés !
			squares = new Square[nodeCountX-1, nodeCountY-1];
			for(int x = 0 ; x<nodeCountX-1; x++){
				for(int y = 0; y<nodeCountY-1 ; y++){
					squares[x,y] = new Square(controlNodes[x,y+1],controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
				}
			}
		}
	}

	/**
	 *Créer des carrés avec les 4 coins(Controle Node) et les Nodes
	 *la configuration refer aux 16 configuration selon les controles nodes actifs
	 * 
	*/
	public class Square{ 
		public ControlNode topLeft, topRight, bottomLeft, bottomRight;
		public Node centerTop, centerRight, centerLeft, centerBottom;
		public int configuration;

		public Square(ControlNode _topLeft,ControlNode _topRight, ControlNode _bottomRight,ControlNode _bottomLeft){
			topLeft = _topLeft;
			topRight = _topRight;
			bottomLeft = _bottomLeft;
			bottomRight = _bottomRight;

			centerTop = topLeft.right;
			centerRight = bottomRight.above;
			centerBottom = bottomLeft.right;
			centerLeft = bottomLeft.above;

			if(topLeft.active){
				configuration +=8;
			}
			if(topRight.active){
				configuration +=4;
			}
			if(bottomRight.active){
				configuration += 2;
			}
			if(bottomLeft.active){
				configuration +=1;
			}
		}
	}

	/*
	 * point du milieu d'un axe entre deux Controle Node d'un carré
	 */
	public class Node{
	
		public Vector3 position;
		public int vertexIndex =-1;

		public Node(Vector3 _pos){
			position = _pos;
		}
	}

	/*
	 * Coin d'un carré avec les deux Nodes qu'il controle
	 */
	public class ControlNode : Node{

		public bool active; // is Wall
		public Node above, right;

		public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos){
			active = _active;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize/2f);
		}
	}

	/*
	void OnDrawGizmos(){ //Affichage !
		if (squareGrid != null) {
			for (int x = 0; x < squareGrid.squares.GetLength(0); x++) {
				for (int y = 0; y < squareGrid.squares.GetLength(1); y++) {
					Gizmos.color = (squareGrid.squares [x, y].topLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topLeft.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].topRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topRight.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomRight.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomLeft.position, Vector3.one * .4f);

					Gizmos.color = Color.grey;
					Gizmos.DrawCube (squareGrid.squares [x, y].centerTop.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerRight.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerBottom.position, Vector3.one * .15f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centerLeft.position, Vector3.one * .15f);


				}

			}
		}
	}*/
}
