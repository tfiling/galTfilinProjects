import javax.swing.ImageIcon;


public class TileBank {
	static Tile tile_Empty = new Tile(0 ,null , null );
	Tile tile2 = null; 
	Tile tile4 = null; 
	Tile tile8 = null; 
	Tile tile16 = null; 
	Tile tile32 = null; 
	Tile tile64 = null; 
	Tile tile128 = null; 
	Tile tile256 = null; 
	Tile tile512 = null; 
	Tile tile1024 = null; 
	Tile tile2048 = null; 
	Tile tile4096 = null; 
	Tile tile8192 = null; 
	Tile tile16384 = null; 

public TileBank(String type)
{
		this.tile2= new Tile(2 ,new ImageIcon(type + "2.PNG"));
	
		this.tile4= new Tile(4 , new ImageIcon(type + "4.PNG"));
		tile2.setNext(tile4);
		
		this.tile8= new Tile(8 ,new ImageIcon(type + "8.PNG"));
		tile4.setNext(tile8);
		
		this.tile16= new Tile(16, new ImageIcon(type + "16.PNG"));
		tile8.setNext(tile16);
		
		this.tile32= new Tile(32,new ImageIcon(type + "32.PNG"));
		tile16.setNext(tile32);
		
		this. tile64= new Tile(64,new ImageIcon(type + "64.PNG"));
		tile32.setNext(tile64);
		
		this. tile128= new Tile(128,new ImageIcon(type + "128.PNG"));
		tile64.setNext(tile128);
		
		this. tile256= new Tile(256,new ImageIcon(type + "256.PNG"));
		tile128.setNext(tile256);
		
		this. tile512= new Tile(512 ,new ImageIcon(type + "512.PNG"));
		tile256.setNext(tile512);
		
		this. tile1024= new Tile(1024, new ImageIcon(type + "1024.PNG"));
		tile512.setNext(tile1024);
		
		this. tile2048= new Tile(2048,new ImageIcon(type + "2048.PNG"));
		tile1024.setNext(tile2048);
		
		this. tile4096= new Tile(4096 ,new ImageIcon(type + "4096.PNG"));
		tile2048.setNext(tile4096);
		
		this. tile8192= new Tile(8192 ,new ImageIcon(type + "8192.PNG"));
		tile4096.setNext(tile8192);
		
		this. tile16384= new Tile(16384 ,new ImageIcon(type + "16384.PNG"));
		tile8192.setNext(tile16384);
			
			
	
}

}