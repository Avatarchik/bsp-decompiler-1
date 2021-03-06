using System;
// Vertex class
// Constains all data necessary to handle a vertex in any BSP format.

public class Vertex:LumpObject {
	
	// INITIAL DATA DECLARATION AND DEFINITION OF CONSTANTS
	
	private Vector3D vertex;
	private float[] texCoord = new float[] { System.Single.NaN, System.Single.NaN };
	
	// CONSTRUCTORS

	public Vertex():base(new byte[0]) { }

	public Vertex(Vector3D vertex):base(new byte[0]) {
		this.vertex = vertex;
	}

	public Vertex(Vector3D vertex, float texCoordU, float texCoordV):base(new byte[0]) {
		this.vertex = vertex;
		this.texCoord[0] = texCoordU;
		this.texCoord[1] = texCoordV;
	}

	public Vertex(LumpObject data, mapType type):base(data.Data) {
		new Vertex(data.Data, type);
	}
	
	public Vertex(byte[] data, mapType type):base(data) {
		switch (type) {
			case mapType.TYPE_DOOM: 
			case mapType.TYPE_HEXEN: 
				vertex = new Vector3D(DataReader.readShort(data[0], data[1]), DataReader.readShort(data[2], data[3]));
				break;
			case mapType.TYPE_STEF2: 
			case mapType.TYPE_MOHAA: 
			case mapType.TYPE_STEF2DEMO: 
			case mapType.TYPE_RAVEN: 
			case mapType.TYPE_QUAKE3: 
			case mapType.TYPE_COD: 
			case mapType.TYPE_FAKK: 
				texCoord[0] = DataReader.readFloat(data[12], data[13], data[14], data[15]);
				texCoord[1] = DataReader.readFloat(data[16], data[17], data[18], data[19]);
				goto case mapType.TYPE_QUAKE;
			case mapType.TYPE_QUAKE: 
			case mapType.TYPE_NIGHTFIRE: 
			case mapType.TYPE_SIN: 
			case mapType.TYPE_SOF: 
			case mapType.TYPE_SOURCE17: 
			case mapType.TYPE_SOURCE18: 
			case mapType.TYPE_SOURCE19: 
			case mapType.TYPE_SOURCE20: 
			case mapType.TYPE_SOURCE21: 
			case mapType.TYPE_SOURCE22: 
			case mapType.TYPE_SOURCE23: 
			case mapType.TYPE_SOURCE27: 
			case mapType.TYPE_TACTICALINTERVENTION: 
			case mapType.TYPE_QUAKE2: 
			case mapType.TYPE_DAIKATANA: 
			case mapType.TYPE_VINDICTUS: 
			case mapType.TYPE_DMOMAM: 
				vertex = DataReader.readPoint3F(data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11]);
				break;
		}
	}
	
	// METHODS
	public static Lump<Vertex> createLump(byte[] data, mapType type) {
		int structLength = 0;
		switch(type) {
			case mapType.TYPE_DOOM: 
			case mapType.TYPE_HEXEN: 
				structLength = 4;
				break;
			case mapType.TYPE_QUAKE: 
			case mapType.TYPE_NIGHTFIRE: 
			case mapType.TYPE_SIN: 
			case mapType.TYPE_SOF: 
			case mapType.TYPE_SOURCE17: 
			case mapType.TYPE_SOURCE18: 
			case mapType.TYPE_SOURCE19: 
			case mapType.TYPE_SOURCE20: 
			case mapType.TYPE_SOURCE21: 
			case mapType.TYPE_SOURCE22: 
			case mapType.TYPE_SOURCE23: 
			case mapType.TYPE_SOURCE27: 
			case mapType.TYPE_TACTICALINTERVENTION: 
			case mapType.TYPE_QUAKE2: 
			case mapType.TYPE_DAIKATANA: 
			case mapType.TYPE_VINDICTUS: 
			case mapType.TYPE_DMOMAM: 
				structLength = 12;
				break;
			case mapType.TYPE_STEF2: 
			case mapType.TYPE_MOHAA: 
			case mapType.TYPE_STEF2DEMO: 
			case mapType.TYPE_QUAKE3: 
			case mapType.TYPE_COD: 
			case mapType.TYPE_FAKK: 
				structLength = 44;
				break;
			case mapType.TYPE_RAVEN: 
				structLength = 80;
				break;
			default: 
				structLength = 0; // This will cause the shit to hit the fan.
				break;
		}
		int offset=0;
		Lump<Vertex> lump = new Lump<Vertex>(data.Length, structLength, data.Length / structLength);
		byte[] bytes=new byte[structLength];
		for(int i=0;i<data.Length / structLength;i++) {
			for (int j=0;j<structLength;j++) {
				bytes[j]=data[offset+j];
			}
			lump.Add(new Vertex(bytes, type));
			offset+=structLength;
		}
		return lump;
	}

	public virtual string ToString() {
		return "( "+vertex.X+" "+vertex.Y+" "+vertex.Z+" "+texCoord[0]+" "+texCoord[1]+" )";
	}
	
	// ACCESSORS/MUTATORS
	public virtual float TexCoordX {
		get {
			return texCoord[0];
		}
	}
	public virtual float TexCoordY {
		get {
			return texCoord[1];
		}
	}
	public virtual Vector3D Vector {
		get {
			return vertex;
		}
	}

	public double X { get { return vertex.X; } set { vertex.X = value; } }
	public double Y { get { return vertex.Y; } set { vertex.Y = value; } }
	public double Z { get { return vertex.Z; } set { vertex.Z = value; } }
	public double this[int index] { get { return vertex[index]; } set { vertex[index]=value; } }
}