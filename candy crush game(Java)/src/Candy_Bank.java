import javax.swing.ImageIcon;


public class Candy_Bank {
	
	static ImageIcon crushed = new ImageIcon("icons\\crushed.PNG");
		
		//regular candies: 
	
	static ImageIcon empty = new ImageIcon("icons\\empty.PNG");
	static ImageIcon candy1 = new ImageIcon("icons\\blue.PNG");
	static ImageIcon candy2 = new ImageIcon("icons\\green.PNG");
	static ImageIcon candy3 = new ImageIcon("icons\\orange.PNG");
	static ImageIcon candy4 = new ImageIcon("icons\\red.PNG");
	static ImageIcon candy5 = new ImageIcon("icons\\yellow.PNG");
	static ImageIcon candy6 = new ImageIcon("icons\\purple.PNG");
	 
	 	// wrapped candies: 
	 
	static ImageIcon candywrapped1 = new ImageIcon("icons\\blue3.PNG"); 
	static ImageIcon candywrapped2 = new ImageIcon("icons\\green3.PNG"); 
	static ImageIcon candywrapped3 = new ImageIcon("icons\\orange4.PNG"); 
	static ImageIcon candywrapped4 = new ImageIcon("icons\\red3.PNG"); 
	static ImageIcon candywrapped5 = new ImageIcon("icons\\yellow3.PNG"); 
	static ImageIcon candywrapped6 = new ImageIcon("icons\\purple3.PNG"); 
	 
	 	// striped vertical candies: 
	 
	static ImageIcon candyVertical1 = new ImageIcon("icons\\blue1.PNG");
	static ImageIcon candyVertical2 = new ImageIcon("icons\\green1.PNG");
	static ImageIcon candyVertical3 = new ImageIcon("icons\\orange1.PNG");
	static ImageIcon candyVertical4 = new ImageIcon("icons\\red1.PNG");
	static ImageIcon candyVertical5 = new ImageIcon("icons\\yellow1.PNG");
	static ImageIcon candyVertical6 = new ImageIcon("icons\\purple1.PNG"); 	
	 
	 	//striped horizon candies:
	static ImageIcon candyHorizon1 = new ImageIcon("icons\\blue2.PNG");
	static ImageIcon candyHorizon2 = new ImageIcon("icons\\green2.PNG");
	static ImageIcon candyHorizon3 = new ImageIcon("icons\\orange2.PNG");
	static ImageIcon candyHorizon4 = new ImageIcon("icons\\red2.PNG");
	static ImageIcon candyHorizon5 = new ImageIcon("icons\\yellow2.PNG");
	static ImageIcon candyHorizon6 = new ImageIcon("icons\\purple2.PNG"); 
	 
	 
	 	// color bomb
	static ImageIcon colorBomb = new ImageIcon("icons\\candycandy.PNG"); 
	
	

	public Candy_Bank()
	{
	 
	}
	
	public static ImageIcon getIcon(int color)
	{
		switch (color)
		{
			// regular candies:
			case 1: 
			{
				return candy1;
				 
			}
			case 2: 
			{
				return candy2;
				 
			}
			case 3: 
			{
				return candy3;
				 
			}
			case 4: 
			{
				return candy4;
				 
			}
			case 5: 
			{
				return candy5;
				 
			}
			case 6: 
			{
				return candy6;
				 
			}
			//vertical striped candies: 
			case 11: 
			{
				return candyVertical1 ;
				 
			}
			case 21: 
			{
				return candyVertical2 ;
				 
			}
			case 31: 
			{
				return candyVertical3 ;
				 
			}
			case 41: 
			{
				return candyVertical4;
				 
			}
			case 51: 
			{
				return candyVertical5;
				 
			}
			case 61: 
			{
				return candyVertical6;
				 
			}
			// horizontal striped candies
			case 12: 
			{
				return candyHorizon1;
				 
			}
			case 22: 
			{
				return candyHorizon2;
				 
			}
			case 32: 
			{
				return candyHorizon3;
				 
			}
			case 42: 
			{
				return candyHorizon4;
				 
			}
			case 52: 
			{
				return candyHorizon5;
				 
			}
			case 62: 
			{
				return candyHorizon6;
				 
			}
			// wrapped candies: 
			
			case 13: 
			{
				return candywrapped1;
				 
			}
			case 23: 
			{
				return candywrapped2;
				 
			}
			case 33: 
			{
				return candywrapped3;
				 
			}
			case 43: 
			{
				return candywrapped4;
				 
			}
			case 53: 
			{
				return candywrapped5;
				 
			}
			case 63: 
			{
				return candywrapped6;
				 
			}
			// color bomb:
			case 10: 
			{
				return colorBomb;
				 
			}
			case 0:
			{
				return empty;
			}
		}
		return null;
	}
}
