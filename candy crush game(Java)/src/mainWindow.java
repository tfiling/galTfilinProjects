import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.GridLayout;
import java.awt.Point;
import java.awt.Window;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.border.BevelBorder;


public class mainWindow extends JFrame implements  MouseListener, ActionListener {
	
	final static int SIZE = 9; 
	private Candy[][] candies;
	private Board itsBoardLogics;
	private PlayBoard itsPlayBoard;
	private int pressedX;
	private int CurrentX;
	private int pressedY;
	private int CurrentY;
	private int turn;
	private int score;
	private JLabel lblMovesCount;
	private JButton btnNewGame;
	private JButton btnRecords;
	private JLabel lblScore;
	
	public mainWindow ()
	{
		super("Candy Crush");
		
		setResizable(false);
		setSize(450, 450);
		this.setVisible(true);
		getContentPane().setLayout(new BorderLayout());
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	    addMouseListener(this);
	    
		this.itsPlayBoard = new PlayBoard();
		itsBoardLogics = itsPlayBoard.itsBoardLogics;
		
	 	candies=itsPlayBoard.board;
	 	this.getContentPane().add(this.itsPlayBoard, BorderLayout.CENTER);

	 	
		JPanel menuBar = new JPanel();
		menuBar.setBorder(new BevelBorder(BevelBorder.RAISED, null, null, null, null));
		menuBar.setBackground(new Color(240, 240, 215));
		getContentPane().add(menuBar, BorderLayout.NORTH);
		menuBar.setLayout(new GridLayout(0, 1, 0, 0));
    
		menuBar.setSize(WIDTH, 50);
       
		this.btnNewGame = new JButton("New game");
		btnNewGame.addActionListener(this);
		btnNewGame.setBackground(new Color(240, 250, 215));
		btnNewGame.setFocusable(false);
       	menuBar.add(btnNewGame);
       
       	this.btnRecords = new JButton("Records Table");
       	btnRecords.setFocusable(false);
       	btnRecords.addActionListener(this);
       	menuBar.add(btnRecords);

       
       	JPanel statsPanel = new JPanel();
       	statsPanel.setBorder(new BevelBorder(BevelBorder.RAISED, null, null, null, null));
       	menuBar.add(statsPanel, BorderLayout.NORTH);
       
        lblScore = new JLabel("Score:");
        lblScore.setText("Score: " + String.valueOf(score));
       	statsPanel.add(lblScore);
       
       
       	lblMovesCount = new JLabel("turn");
       	lblMovesCount.setText("moves: " + String.valueOf(turn));
       	statsPanel.add(lblMovesCount);
       	

       	
       	
		
       	this.pack();

		
	}
	
	public static void main(String args[])
	{
		mainWindow frame = new mainWindow();
	}

	public void playGame ()
	{
		
		if ( (pressedX!= CurrentX || pressedY != CurrentY )  && turn <= 20)
		{
			// get the pressed point location on the screen:
			Point location1= new Point(pressedX, pressedY);
			Point location2= new Point(CurrentX, CurrentY);			
			 			
			// get the labels pressed:
			Point locate1 = getLableIndexesByLocation(location1);
			Point locate2 = getLableIndexesByLocation(location2);
			
			// check for wrong input: 
			if  ( locate1!=null && locate2!=null
				&& ( ( Math.abs((locate1.getX()-locate2.getX()))==1 &&  Math.abs((locate1.getY()-locate2.getY()))==0 ) 
			    ||  ( Math.abs((locate1.getY()-locate2.getY())) == 1 &&  Math.abs((locate1.getX()-locate2.getX()))== 0 ) ) )
				// because the two indexes of the labels in the array must be adjacent
			{
			
					// now locate1 and locate2 are the indexes of the candies array
					// in which the movement occurred
					
					// the candies which pressed:
					Candy candy_this = candies[locate1.x][locate1.y];
					Candy candy_other = candies[locate2.x][locate2.y];
					
					// validate they have the updated board:
					candy_this.setBoard((candies));
					candy_other.setBoard((candies));
					
					// try move on a virtual board:
					Candy[][] temp = deepCopy(candies);
					candy_this.setBoard(temp);
					candy_this.combine(candy_other);
					
					int Tempscore = itsBoardLogics.updateScoreAfterMove(temp);
					if (Tempscore>=30) // candy_this created crush
					{
						candies=temp;
						candy_this = candies[locate2.x][locate2.y];
						candy_other = candies[locate1.x][locate1.y];
					}
					else // the board hasn't changed
					{
						temp = deepCopy(candies);
						candy_other.setBoard(temp);
						candy_this.setBoard(temp);
					}
					
					
					if (candy_other.getColor() != 0)
					{
						Tempscore = 0;
						candy_other.combine(candy_this);
						Tempscore = itsBoardLogics.updateScoreAfterMove(temp);
						if (Tempscore>=30)//candy_other created crush
						{
							candies=temp;
						}
						else
						// there were no crushes
						{
							//do nothing
							return;
						}
					}
					
					itsPlayBoard.setBoard(candies);
					
					this.score = this.score + this.itsPlayBoard.updateBoardAfterMove();
					
					candies = itsPlayBoard.board;
					
						 
					this.getContentPane().add(this.itsPlayBoard, BorderLayout.CENTER);
					this.setVisible(true);
					this.turn++;
					lblScore.setText("Score: " + String.valueOf(score));
					lblMovesCount.setText("moves: " + String.valueOf(turn));
			}
			else
			{
				JOptionPane.showMessageDialog
					(this,	"wrong movement , please try again.");
			}
		}
		if (turn >=20)
		{
			JOptionPane.showMessageDialog
			(this,	"Game Over! your score is  " + score);
			String name = JOptionPane.showInputDialog(this, "Game Over, your score is: " + score + System.getProperty("line.separator") + "please insert your name");
			HighScores.insertScore(Integer.toString(this.score), name);
		}
	
	}
	


	private Candy[][] deepCopy(Candy[][] candies2)
	{
		Candy[][] toReturn = new Candy[SIZE][SIZE];
		for ( int i = 0 ; i <=SIZE-1 ; i++)
		{
			for ( int j = 0 ; j<=SIZE-1 ; j++)
			{
				toReturn[i][j]=candies2[i][j];
				toReturn[i][j]= candies2[i][j];
				toReturn[i][j].setBoard(toReturn);
				toReturn[i][j].setColumn(i);
				toReturn[i][j].setRow(j);
				toReturn[i][j].setColor(candies2[i][j].getColor());
			}
		}
		return toReturn;
	}

	private Point getLableIndexesByLocation(Point location) {
		for ( int i = 0 ; i <=SIZE-1 ; i++)
		{
			for ( int j = 0 ; j<=SIZE-1 ; j++)
			{
				if (pointInTheRange(location,i,j))
				{
					Point toReturn  = new Point(i, j);
					// i = col , j = row
					return toReturn;
				}
			}
		}
		return null;
	}

	
	
	private boolean pointInTheRange(Point location, int col, int row)
	{
		if ( location.x >= col*50 && location.x <= col*50 +50 )
		{
			if (location.y >= row*50+120 && location.y <= row*50+170 )
			{
				return true;
			}
		}

		return false;
	}

	@Override
	public void mouseClicked(MouseEvent arg0) {
		// Auto-generated method stub
		
	}

	@Override
	public void mouseEntered(MouseEvent arg0) {
		// Auto-generated method stub
		
	}

	@Override
	public void mouseExited(MouseEvent arg0) {
		// Auto-generated method stub
		
	}

	@Override
	public void mousePressed(MouseEvent event) {
		Point point = event.getPoint();
		this.pressedX = point.x;
		this.pressedY = point.y;

	}

	@Override
	public void mouseReleased(MouseEvent arg0) {

		Point p = arg0.getPoint();
		this.CurrentX = p.x;
		this.CurrentY = p.y;
 		playGame();
		
	}


	@Override
	public void actionPerformed(ActionEvent e) {
		if (e.getSource() == this.btnNewGame)
		{
			mainWindow frame = new mainWindow();
			this.dispose();
			
		}
		else if (e.getSource() == this.btnRecords)
		{
			HighScores highScores = new HighScores();
    		highScores.updateTable();
    		highScores.setVisible(true);
		}
		
	}
}
