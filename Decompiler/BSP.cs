using System;
using System.Collections.Generic;
// BSP class
// Holds data for any and all BSP formats. Any unused lumps in a given format
// will be left as null. Then it will be fed into a universal decompile method
// which should be able to perform its job based on what data is stored.

public enum mapType : int {
	TYPE_UNDEFINED = 0,
	// Bunch of different versions. Can be used to differentiate maps or strucures.
	TYPE_QUAKE = 29,
	// TYPE_GOLDSRC = 30, // Uses same algorithm and structures as Quake
	TYPE_NIGHTFIRE = 42,
	TYPE_VINDICTUS = 346131372,
	TYPE_STEF2 = 556942937,
	TYPE_MOHAA = 892416069,
	// TYPE_MOHBT = 1095516506, // Similar enough to MOHAA to use the same structures and algorithm
	TYPE_DOOM = 1145132868, // "DWAD"
	TYPE_HEXEN = 1145132872, // "HWAD"
	TYPE_STEF2DEMO = 1263223129,
	TYPE_FAKK = 1263223152,
	TYPE_TACTICALINTERVENTION = 1268885814,
	TYPE_COD2 = 1347633741, // Uses same algorithm and structures as COD1. Read differently.
	TYPE_SIN = 1347633747, // The headers for SiN and Jedi Outcast are exactly the same
	TYPE_RAVEN = 1347633748,
	TYPE_COD4 = 1347633759, // Uses same algorithm and structures as COD1. Read differently.
	TYPE_SOURCE17 = 1347633767,
	TYPE_SOURCE18 = 1347633768,
	TYPE_SOURCE19 = 1347633769,
	TYPE_SOURCE20 = 1347633770,
	TYPE_SOURCE21 = 1347633771,
	TYPE_SOURCE22 = 1347633772,
	TYPE_SOURCE23 = 1347633773,
	TYPE_QUAKE2 = 1347633775,
	TYPE_SOURCE27 = 1347633777,
	TYPE_DAIKATANA = 1347633778,
	TYPE_SOF = 1347633782, // Uses the same header as Q3.
	TYPE_QUAKE3 = 1347633783,
	// TYPE_RTCW = 1347633784, // Uses same algorithm and structures as Quake 3
	TYPE_COD = 1347633796,
	TYPE_DMOMAM = 1347895914
}

public class BSP {
	// INITIAL DATA DECLARATION AND DEFINITION OF CONSTANTS
	
	// What kind is this map?
	private mapType version;
	
	private string filePath;
	
	// Map structures
	// Quake 1/GoldSrc
	private Entities entities;
	private Lump<Plane> planes;
	private Textures textures;
	private Lump<Vertex> vertices;
	private Lump<Node> nodes;
	private Lump<TexInfo> texInfo;
	private Lump<Face> faces;
	private Lump<Leaf> leaves;
	private NumList markSurfaces;
	private Lump<Edge> edges;
	private NumList surfEdges;
	private Lump<Model> models;
	// Quake 2
	private Lump<Brush> brushes;
	private Lump<BrushSide> brushSides;
	private NumList markBrushes;
	// MOHAA
	//private MoHAAStaticProps staticProps;
	// Nightfire
	private Textures materials;
	// Source
	private Lump<Face> originalFaces;
	private NumList texTable;
	private Lump<SourceTexData> texDatas;
	private Lump<SourceDispInfo> dispInfos;
	private SourceDispVertices dispVerts;
	private NumList displacementTriangles;
	private SourceStaticProps staticProps;
	private Lump<SourceCubemap> cubemaps;
	//private SourceOverlays overlays;

	// CONSTRUCTORS
	public BSP(string filePath, mapType version) {
		this.filePath = filePath;
		this.version = version;
	}
	
	// METHODS
	
	public virtual void printBSPReport() {
		// If there's a NullReferenceException here, the BSPReader class didn't initialize the object and therefore
		// this is either a BSP format which doesn't use that lump, or there's an error which will become apparent.
		DecompilerThread.OnMessage(this, "Internal version number: "+(int)version+" ("+version+")");
		if(entities != null) {
			DecompilerThread.OnMessage(this, "Entities lump: " + entities.Length + " bytes, " + entities.Count + " items");
		}
		if(planes != null) {
			DecompilerThread.OnMessage(this, "Planes lump: " + planes.Length + " bytes, " + planes.Count + " items");
			if (planes.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Planes");
			}
		}
		if(textures != null) {
			DecompilerThread.OnMessage(this, "Texture lump: " + textures.Length + " bytes, " + textures.Count + " items");
			if (textures.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Texture");
			}
		}
		if(materials != null) {
			DecompilerThread.OnMessage(this, "Materials lump: " + materials.Length + " bytes, " + materials.Count + " items");
			if (materials.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Materials");
			}
		}
		if(vertices != null) {
			DecompilerThread.OnMessage(this, "Vertices lump: " + vertices.Length + " bytes, " + vertices.Count + " items");
			if (vertices.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Vertices");
			}
		}
		if(nodes != null) {
			DecompilerThread.OnMessage(this, "Nodes lump: " + nodes.Length + " bytes, " + nodes.Count + " items");
			if (nodes.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Nodes");
			}
		}
		if(texInfo != null) {
			DecompilerThread.OnMessage(this, "Texture info lump: " + texInfo.Length + " bytes, " + texInfo.Count + " items");
			if (texInfo.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Texture info");
			}
		}
		if(faces != null) {
			DecompilerThread.OnMessage(this, "Faces lump: " + faces.Length + " bytes, " + faces.Count + " items");
			if (faces.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Faces");
			}
		}
		if(leaves != null) {
			DecompilerThread.OnMessage(this, "Leaves lump: " + leaves.Length + " bytes, " + leaves.Count + " items");
			if (leaves.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Leaves");
			}
		}
		if(markSurfaces != null) {
			DecompilerThread.OnMessage(this, "Mark surfaces lump: " + markSurfaces.Length + " bytes, " + markSurfaces.Count + " items");
			if (markSurfaces.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Mark surfaces");
			}
		}
		if(edges != null) {
			DecompilerThread.OnMessage(this, "Edges lump: " + edges.Length + " bytes, " + edges.Count + " items");
			if (edges.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Edges");
			}
		}
		if(surfEdges != null) {
			DecompilerThread.OnMessage(this, "Surface Edges lump: " + surfEdges.Length + " bytes, " + surfEdges.Count + " items");
			if (surfEdges.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Surface Edges");
			}
		}
		if(models != null) {
			DecompilerThread.OnMessage(this, "Models lump: " + models.Length + " bytes, " + models.Count + " items");
			if (models.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Models");
			}
		}
		if(brushes != null) {
			DecompilerThread.OnMessage(this, "Brushes lump: " + brushes.Length + " bytes, " + brushes.Count + " items");
			if (brushes.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Brushes");
			}
		}
		if(brushSides != null) {
			DecompilerThread.OnMessage(this, "Brush sides lump: " + brushSides.Length + " bytes, " + brushSides.Count + " items");
			if (brushSides.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Brush sides");
			}
		}
		if(markBrushes != null) {
			DecompilerThread.OnMessage(this, "Mark brushes lump: " + markBrushes.Length + " bytes, " + markBrushes.Count + " items");
			if (markBrushes.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Mark brushes");
			}
		}
		if(originalFaces != null) {
			DecompilerThread.OnMessage(this, "Original Faces lump: " + originalFaces.Length + " bytes, " + originalFaces.Count + " items");
			if (originalFaces.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Original Faces");
			}
		}
		if(texTable != null) {
			DecompilerThread.OnMessage(this, "Texture index table lump: " + texTable.Length + " bytes, " + texTable.Count + " items");
			if (texTable.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Texture index table");
			}
		}
		if(texDatas != null) {
			DecompilerThread.OnMessage(this, "Texture data lump: " + texDatas.Length + " bytes, " + texDatas.Count + " items");
			if (texDatas.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Texture data");
			}
		}
		if(dispInfos != null) {
			DecompilerThread.OnMessage(this, "Displacement info lump: " + dispInfos.Length + " bytes, " + dispInfos.Count + " items");
			if (dispInfos.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Displacement info");
			}
		}
		if(dispVerts != null) {
			DecompilerThread.OnMessage(this, "Displacement Vertices lump: " + dispVerts.Length + " bytes, " + dispVerts.Count + " items");
			if (dispVerts.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Displacement Vertices");
			}
		}
		if(displacementTriangles != null) {
			DecompilerThread.OnMessage(this, "Displacement Triangle Tags lump: " + displacementTriangles.Length + " bytes, " + displacementTriangles.Count + " items");
			if (displacementTriangles.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Displacement Triangle Tags");
			}
		}
		if(staticProps != null) {
			DecompilerThread.OnMessage(this, "Static Props lump: " + staticProps.Length + " bytes, " + staticProps.Count + " items, " + staticProps.Dictionary.Length + " unique models");
		}
		if(cubemaps != null) {
			DecompilerThread.OnMessage(this, "Cubemaps lump: " + cubemaps.Length + " bytes, " + cubemaps.Count + " items");
			if (cubemaps.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Cubemaps");
			}
		} /*
		if(overlays != null) {
		DecompilerThread.OnMessage(this, "Overlays lump: "+overlays.Length+" bytes, "+overlays.Count+" items",Settings.VERBOSITY_MAPSTATS);
			if(overlays.hasFunnySize()) {
				DecompilerThread.OnMessage(this, "WARNING: Funny lump size in Overlays",Settings.VERBOSITY_WARNINGS);
			}
		} catch(System.NullReferenceException) {
		}*/
	}
	
	// +getLeavesInModel(int)
	// Returns an array of Leaf containing all the leaves referenced from
	// this model's head node. This array cannot be referenced by index numbers
	// from other lumps, but if simply iterating through, getting information
	// it'll be just fine.
	public virtual Leaf[] getLeavesInModel(int model) {
		int head = models[model].HeadNode;
		if(head < 0) {
			//head = Math.Abs(head);
			DecompilerThread.OnMessage(this, "WARNING: Model "+model+" links to a negative node!");
		}
		return getLeavesInNode(head);
	}
	
	// +getLeavesInNode(int)
	// Returns an array of Leaf containing all the leaves referenced from
	// this node. Since nodes reference other nodes, this may recurse quite
	// some ways. Eventually every node will boil down to a set of leaves,
	// which is what this method returns.
	
	// This is an iterative preorder traversal algorithm modified from the Wikipedia page at:
	// http://en.wikipedia.org/wiki/Tree_traversal on April 19, 2012.
	// The cited example has since been removed but can still be found at
	// http://en.wikipedia.org/w/index.php?title=Tree_traversal&oldid=488219889#Inorder_Traversal
	// I needed an iterative algorithm because recursive ones commonly gave stack overflows.
	public virtual Leaf[] getLeavesInNode(int head) {
		Node headNode;
		Leaf[] nodeLeaves = new Leaf[0];
		if(head<0) { // WHY does this happen? What does it mean if this is negative?
			return nodeLeaves;
		} else {
			headNode = nodes[head];
		}
		Stack<Node> nodestack = new Stack<Node>();
		nodestack.Push(headNode);
		
		Node currentNode;
		
		while (!(nodestack.Count == 0)) {
			currentNode = nodestack.Pop();
			int right = currentNode.Child2;
			if (right >= 0) {
				nodestack.Push(nodes[right]);
			} else {
				Leaf[] newList = new Leaf[nodeLeaves.Length + 1];
				for (int i = 0; i < nodeLeaves.Length; i++) {
					newList[i] = nodeLeaves[i];
				}
				newList[nodeLeaves.Length] = leaves[(right * (- 1)) - 1]; // Quake 2 subtracts 1 from the index
				nodeLeaves = newList;
			}
			int left = currentNode.Child1;
			if (left >= 0) {
				nodestack.Push(nodes[left]);
			} else {
				Leaf[] newList = new Leaf[nodeLeaves.Length + 1];
				for (int i = 0; i < nodeLeaves.Length; i++) {
					newList[i] = nodeLeaves[i];
				}
				newList[nodeLeaves.Length] = leaves[(left * (- 1)) - 1]; // Quake 2 subtracts 1 from the index
				nodeLeaves = newList;
			}
		}
		return nodeLeaves;
	}
	
	// Only for Source engine.
	public virtual int findTexDataWithTexture(string texture) {
		for (int i = 0; i < texDatas.Count; i++) {
			string temp = textures.getTextureAtOffset((uint)texTable[texDatas[i].StringTableIndex]);
			if (temp.Equals(texture)) {
				return i;
			}
		}
		return - 1;
	}

	// ACCESSORS/MUTATORS
	public virtual string Path {
		get {
			return filePath;
		}
	}

	public virtual string MapName {
		get {
			int i;
			for (i = 0; i < filePath.Length; i++) {
				if (filePath[filePath.Length - 1 - i] == '\\') {
					break;
				}
				if (filePath[filePath.Length - 1 - i] == '/') {
					break;
				}
			}
			return filePath.Substring(filePath.Length - i, (filePath.Length) - (filePath.Length - i));
		}
	}

	public virtual string MapNameNoExtension {
		get {
			string name = MapName;
			int i;
			for (i = 0; i < name.Length; i++) {
				if (name[name.Length - 1 - i] == '.') {
					break;
				}
			}
			return name.Substring(0, (name.Length - 1 - i) - (0));
		}
	}

	public virtual string Folder {
		get {
			int i;
			for (i = 0; i < filePath.Length; i++) {
				if (filePath[filePath.Length - 1 - i] == '\\') {
					break;
				}
				if (filePath[filePath.Length - 1 - i] == '/') {
					break;
				}
			}
			return filePath.Substring(0, (filePath.Length - i) - (0));
		}
	}

	public virtual mapType Version {
		get {
			return version;
		}
		set {
			version = value;
		}
	}

	public virtual Lump<Plane> Planes {
		set {
			planes = value;
		}
		get {
			return planes;
		}
	}

	public virtual Lump<Vertex> Vertices {
		set {
			vertices = value;
		}
		get {
			return vertices;
		}
	}

	public virtual Lump<Node> Nodes {
		set {
			nodes = value;
		}
		get {
			return nodes;
		}
	}

	public virtual Lump<TexInfo> TexInfo {
		set {
			texInfo = value;
		}
		get {
			return texInfo;
		}
	}

	public virtual Lump<Face> Faces {
		set {
			faces = value;
		}
		get {
			return faces;
		}
	}

	public virtual Lump<Leaf> Leaves {
		set {
			leaves = value;
		}
		get {
			return leaves;
		}
	}

	public virtual Lump<Model> Models {
		set {
			models = value;
		}
		get {
			return models;
		}
	}

	public virtual Lump<Brush> Brushes {
		set {
			brushes = value;
		}
		get {
			return brushes;
		}
	}

	public virtual Lump<BrushSide> BrushSides {
		set {
			brushSides = value;
		}
		get {
			return brushSides;
		}
	}

	public virtual Lump<SourceTexData> TexDatas {
		set {
			texDatas = value;
		}
		get {
			return texDatas;
		}
	}

	public virtual Lump<SourceDispInfo> DispInfos {
		set {
			dispInfos = value;
		}
		get {
			return dispInfos;
		}
	}

	public virtual Lump<Face> OriginalFaces {
		set {
			originalFaces = value;
		}
		get {
			return originalFaces;
		}
	}

	public virtual Lump<SourceCubemap> Cubemaps {
		set {
			cubemaps = value;
		}
		get {
			return cubemaps;
		}
	}

	public virtual Entities Entities {
		set {
			entities = value;
		}
		get {
			return entities;
		}
	}

	public virtual Textures Textures {
		set {
			textures = value;
		}
		get {
			return textures;
		}
	}

	public virtual Textures Materials {
		set {
			materials = value;
		}
		get {
			return materials;
		}
	}
	
	public virtual NumList MarkSurfaces {
		set {
			//switch (version) {
			//	case TYPE_QUAKE: 
					markSurfaces = value;
			//		markSurfaces = new NumList(data, NumList.TYPE_USHORT);
			//		break;
			//}
		}
		get {
			return markSurfaces;
		}
	}

	public virtual Lump<Edge> Edges {
		get {
			return edges;
		}
		set {
			edges = value;
		}
	}

	public virtual NumList SurfEdges {
		get {
			return surfEdges;
		}
		set {
			surfEdges = value;
		}
	}

	public virtual NumList MarkBrushes {
		get {
			return markBrushes;
		}
		set {
			markBrushes = value;
		}
	}

	public virtual SourceDispVertices DispVerts {
		get {
			return dispVerts;
		}
		set {
			dispVerts = value;
		}
	}

	public virtual NumList TexTable {
		set {
			texTable = value;
			//texTable = new NumList(data, NumList.TYPE_UINT);
		}
		get {
			return texTable;
		}
	}

	public virtual NumList DispTris {
		set {
			displacementTriangles = value;
			//displacementTriangles = new NumList(data, NumList.TYPE_USHORT);
		}
		get {
			return displacementTriangles;
		}
	}

	public virtual SourceStaticProps StaticProps {
		set {
			staticProps = value;
			//staticProps = SourceStaticProp.createLump(data, version, lumpVersion);
		}
		get {
			return staticProps;
		}
	}
}