import javax.swing.ImageIcon;


public class Tile {

	//fields:
	public int numOfTile ; 
	ImageIcon tileIcon;
	Tile next;
	TileBank itsTileBank;
	boolean isAresult;
		
	
	// constructors:
	public Tile(int numOfTile ,ImageIcon tileIcon,  Tile next)
	{
		this.setNext(next);
		this.setNumOfTile(numOfTile);
		this.setTileIcon(tileIcon);
		isAresult=false;
	}
		
	public Tile(int numOfTile ,ImageIcon tileIcon)
	{
		this.setNumOfTile(numOfTile);
		this.setTileIcon(tileIcon);
		isAresult=false;
	}
		

	// logics: 
		
	public boolean equals (Tile tile)
	// checks if the tiles are the same
	{
		if (tile.getNumOfTile() == this.getNumOfTile())
		{
			return true;
		}
		return false;
	}
	
	// getters and setters:
	public int getNumOfTile ()
	{
		return numOfTile;
	}
	public void setNumOfTile (int numOfTile)
	{
		this.numOfTile =  numOfTile;
	}
	public ImageIcon getTileIcon ()
	{
		return tileIcon;
	}
	public void setTileIcon (ImageIcon tileIcon)
	{
		this.tileIcon =  tileIcon;
	}
	public void setNext (Tile next)
	{
		this.next =  next;
	}
	public Tile getNext ()
	{
		return this.next;
	}
	
}
