import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.border.BevelBorder;
import javax.swing.border.LineBorder;




public class GameBoard extends JFrame  implements KeyListener , ActionListener{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private ChangeTheme changeThemeWindow;

	//fields: 
	// final number of the 4*4 board
	private final int SIZE = 4;
	Tile [][]  board;
	TileBank itsTileBank = new TileBank("classic");
	private final JPanel gamePanel = new JPanel();
	private GameBoardLogics itsGameBoardLogics; 
	private String score; 
	
	//those are final because we need to access them in order to change their properties
	private final JLabel lblScoreValue;
	private final JButton buttonNewGame;
	private final JButton buttonChangeTheme;
	private final JButton buttonRecordsTable;
	private final JButton buttonAbout;
	private final JPanel statsPanel;
	private final JLabel lblScore;

	
	//constructor:
	public GameBoard () 
	{
		super("2048");
		setResizable(false);
	
		// build the frame:
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setSize(660,634);
		this.setVisible(true);
		getContentPane().setLayout(new BorderLayout());
	
		//Initialize new board:
		this.board = new Tile[SIZE][SIZE];
		
		this.itsGameBoardLogics = new GameBoardLogics(itsTileBank);
		// build empty board
		this.board = itsGameBoardLogics.buildNewBoard(this.board, itsTileBank);
        gamePanel.setBorder(new BevelBorder(BevelBorder.LOWERED, null, null, null, null));

        gamePanel.setLayout(new GridLayout(4, 4, 0, 0));
        gamePanel.setBackground(new Color(205, 193, 181));
        
        getContentPane().add(this.gamePanel, BorderLayout.CENTER);

        gamePanel.setVisible(true);

		updatBoard(this.board);
		 addKeyListener(this);
		 
		 JPanel menuBar = new JPanel();
		 menuBar.setBorder(null);
		 menuBar.setBackground(new Color(240, 240, 215));
		 getContentPane().add(menuBar, BorderLayout.EAST);
        menuBar.setLayout(new GridLayout(0, 1, 0, 0));
     
        this.buttonNewGame = new JButton("New Game");
        buttonNewGame.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		itsGameBoardLogics = new GameBoardLogics(itsTileBank);
        		board = itsGameBoardLogics.buildNewBoard(board, itsTileBank);
        		updatBoard(board);
        	}
        });
        buttonNewGame.setFont(new Font("Segoe UI", Font.BOLD, 20));
        buttonNewGame.setForeground(Color.WHITE);
        buttonNewGame.setBackground(new Color(160, 140, 120));
        buttonNewGame.setFocusable(false);
        menuBar.add(buttonNewGame);
        
        this.buttonRecordsTable = new JButton("Records Table");
        buttonRecordsTable.setFont(new Font("Segoe UI", Font.BOLD, 20));
        buttonRecordsTable.setForeground(Color.WHITE);
        buttonRecordsTable.setBackground(new Color(160, 140, 120));
        buttonRecordsTable.setFocusable(false);
        buttonRecordsTable.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		HighScores highScores = new HighScores();
        		highScores.updateTable();
        		highScores.setVisible(true);
        	}
        });
        menuBar.add(buttonRecordsTable);
        this.buttonChangeTheme = new JButton("Change Theme");
        buttonChangeTheme.setFont(new Font("Segoe UI", Font.BOLD, 20));
        buttonChangeTheme.setForeground(Color.WHITE);
        buttonChangeTheme.setBackground(new Color(160, 140, 120));
        buttonChangeTheme.setFocusable(false);
        this.changeThemeWindow = new ChangeTheme();
        buttonChangeTheme.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		
        		changeThemeWindow.setVisible(true);
        	}
        });
        buttonChangeTheme.setFocusable(false);
        menuBar.add(buttonChangeTheme);
        
        this.buttonAbout = new JButton("About");
        buttonAbout.setFont(new Font("Segoe UI", Font.BOLD, 20));
        buttonAbout.setForeground(Color.WHITE);
        buttonAbout.setBackground(new Color(160, 140, 120));
        buttonAbout.setFocusable(false);
        buttonAbout.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		About aboutWindow = new About();
        		aboutWindow.setVisible(true);
        	}
        });
        buttonAbout.setFocusable(false);
        menuBar.add(buttonAbout);
        
        this.statsPanel = new JPanel();
        statsPanel.setBackground(new Color(160, 140, 120));
        statsPanel.setBorder(new BevelBorder(BevelBorder.RAISED, null, null, null, null));
        getContentPane().add(statsPanel, BorderLayout.NORTH);
        
        this.lblScore = new JLabel("Score:");
        lblScore.setFont(new Font("Segoe UI", Font.BOLD, 20));
        lblScore.setForeground(Color.WHITE);
        statsPanel.add(lblScore);
        
        this.score = "0";
        this.lblScoreValue = new JLabel();
        lblScoreValue.setFont(new Font("Segoe UI", Font.BOLD, 20));
        lblScoreValue.setForeground(Color.WHITE);
        lblScoreValue.setText(score);
        statsPanel.add(lblScoreValue);

        
         KeyEvent ke = 	 new KeyEvent(buttonNewGame , KeyEvent.KEY_PRESSED,
		 System.currentTimeMillis(),
		 0, KeyEvent.KEY_PRESSED,
		 (char) KeyEvent.KEY_PRESSED);
         
        
	     this.pack();
	     
       

		 //start the game:
		 playGame(ke);

	}
	
	//updates the board according to the repressenting array
	private void updatBoard (Tile [][]  board)
	{
		this.board = board;
			
		this.gamePanel.removeAll();
		
        JLabel label_1_1 = new JLabel();
        label_1_1.setForeground(Color.BLACK);
        label_1_1.setIcon(board[0][0].tileIcon);
        label_1_1.setBorder(new LineBorder(new Color(0, 0, 0)));
        gamePanel.add(label_1_1);
        
        JLabel label_2_1 = new JLabel();
        label_2_1.setIcon(board[1][0].tileIcon);
        label_2_1.setBorder(new LineBorder(new Color(0, 0, 0)));
        gamePanel.add(label_2_1);
        
        JLabel label_3_1 = new JLabel();
        label_3_1.setIcon(board[2][0].tileIcon);
        label_3_1.setBorder(new LineBorder(new Color(0, 0, 0)));
        gamePanel.add(label_3_1);
        
        JLabel label_4_1 = new JLabel();
        label_4_1.setIcon(board[3][0].tileIcon);
        label_4_1.setBorder(new LineBorder(new Color(0, 0, 0)));
        gamePanel.add(label_4_1);
        
        JLabel label_1_2 = new JLabel();
        label_1_2.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_1_2.setIcon(board[0][1].tileIcon);
        gamePanel.add(label_1_2);
        
        JLabel label_2_2 = new JLabel();
        label_2_2.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_2_2.setIcon(board[1][1].tileIcon);
        gamePanel.add(label_2_2);
        
        JLabel label_3_2 = new JLabel();
        label_3_2.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_3_2.setIcon(board[2][1].tileIcon);
        gamePanel.add(label_3_2);
        
        JLabel label_4_2 = new JLabel();
        label_4_2.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_4_2.setIcon(board[3][1].tileIcon);
        gamePanel.add(label_4_2);
        
        JLabel label_1_3 = new JLabel();
        label_1_3.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_1_3.setIcon(board[0][2].tileIcon);
        gamePanel.add(label_1_3);
        
        JLabel label_2_3 = new JLabel();
        label_2_3.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_2_3.setIcon(board[1][2].tileIcon);
        gamePanel.add(label_2_3);
        
        JLabel label_3_3 = new JLabel();
        label_3_3.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_3_3.setIcon(board[2][2].tileIcon);
        gamePanel.add(label_3_3);
        
        JLabel label_4_3 = new JLabel();
        label_4_3.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_4_3.setIcon(board[3][2].tileIcon);
        gamePanel.add(label_4_3);
        
        JLabel label_1_4 = new JLabel();
        label_1_4.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_1_4.setIcon(board[0][3].tileIcon);
        gamePanel.add(label_1_4);
        
        JLabel label_2_4 = new JLabel();
        label_2_4.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_2_4.setIcon(board[1][3].tileIcon);
        gamePanel.add(label_2_4);
        
        JLabel label_3_4 = new JLabel();
        label_3_4.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_3_4.setIcon(board[2][3].tileIcon);
        gamePanel.add(label_3_4);
        
        JLabel label_4_4 = new JLabel();
        label_4_4.setBorder(new LineBorder(new Color(0, 0, 0)));
        label_4_4.setIcon(board[3][3].tileIcon);
        gamePanel.add(label_4_4);
        
        this.score = itsGameBoardLogics.getScore();
        if (this.lblScoreValue != null)
        {
        	this.lblScoreValue.setText(this.score);
        }

        //updates the theme, if it was changed
        if (this.changeThemeWindow != null)
        {
	        this.lblScore.setForeground(this.changeThemeWindow.getTextColor());
			this.lblScoreValue.setForeground(this.changeThemeWindow.getTextColor());
			this.buttonNewGame.setForeground(this.changeThemeWindow.getTextColor());
			this.buttonChangeTheme.setForeground(this.changeThemeWindow.getTextColor());
			this.buttonRecordsTable.setForeground(this.changeThemeWindow.getTextColor());
			this.buttonAbout.setForeground(this.changeThemeWindow.getTextColor());

			this.statsPanel.setBackground(this.changeThemeWindow.getPanelColor());
			this.buttonNewGame.setBackground(this.changeThemeWindow.getPanelColor());
			this.buttonChangeTheme.setBackground(this.changeThemeWindow.getPanelColor());
			this.buttonRecordsTable.setBackground(this.changeThemeWindow.getPanelColor());
			this.buttonAbout.setBackground(this.changeThemeWindow.getPanelColor());
			
			this.gamePanel.setBackground(this.changeThemeWindow.getBoardBackground());
			
			this.itsTileBank = this.changeThemeWindow.getTileBank();

        }
        
		
		this.repaint();
		this.pack();
		
	}
	

	//updates the icons in the representing array according to the player's choice in the change theme window
	private void updateIcons() 
	{
		for (int i = 0; i <= 3; i++)
			for (int j = 0; j <= 3; j++)
			{
				int tilenum = this.board[i][j].numOfTile;
				switch (tilenum)
				{
				case 2:
					this.board[i][j].setTileIcon(this.itsTileBank.tile2.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile4.tileIcon);
				break;
				case 4:
					this.board[i][j].setTileIcon(this.itsTileBank.tile4.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile8.tileIcon);
				break;
				case 8:
					this.board[i][j].setTileIcon(this.itsTileBank.tile8.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile16.tileIcon);
				break;
				case 16:
					this.board[i][j].setTileIcon(this.itsTileBank.tile16.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile32.tileIcon);
				break;
				case 32:
					this.board[i][j].setTileIcon(this.itsTileBank.tile32.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile64.tileIcon);
				break;
				case 64:
					this.board[i][j].setTileIcon(this.itsTileBank.tile64.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile128.tileIcon);
				break;
				case 128:
					this.board[i][j].setTileIcon(this.itsTileBank.tile128.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile256.tileIcon);
				break;
				case 256:
					this.board[i][j].setTileIcon(this.itsTileBank.tile256.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile512.tileIcon);
				break;
				case 512:
					this.board[i][j].setTileIcon(this.itsTileBank.tile512.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile1024.tileIcon);
				break;
				case 1024:
					this.board[i][j].setTileIcon(this.itsTileBank.tile1024.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile2048.tileIcon);
				break;
				case 2048:
					this.board[i][j].setTileIcon(this.itsTileBank.tile2048.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile4096.tileIcon);
				break;
				case 4096:
					this.board[i][j].setTileIcon(this.itsTileBank.tile4096.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile8192.tileIcon);
				break;
				case 8192:
					this.board[i][j].setTileIcon(this.itsTileBank.tile8192.tileIcon);
					this.board[i][j].next.setTileIcon(this.itsTileBank.tile16384.tileIcon);
				break;
				case 16384:
					this.board[i][j].setTileIcon(this.itsTileBank.tile16384.tileIcon);
				break;
				}
			}
		repaint();
		pack();
	}

	private void updatBoard ()
	{
		updatBoard(this.board);
	}
	
	
	private void playGame (KeyEvent e)
	{
		boolean GreatTileExists = false;
		boolean lost = false;
		
			while (!lost)
			{
				keyPressed(e);
				
				if (itsGameBoardLogics.isBoardFull(this.board))
				{
					if (!GreatTileExists && itsGameBoardLogics.IsWinning(this.board))
					{
						GreatTileExists = true;
						JOptionPane.showMessageDialog(this, "congratulations! You reached the 2048 tile!");
					}
					
					lost = !itsGameBoardLogics.isThereMoves(this.board);
				}
				
				if (this.changeThemeWindow.themeChanged)
				{
					this.itsTileBank = changeThemeWindow.getTileBank();
					updateIcons();
					this.changeThemeWindow.themeChanged = false;
					updatBoard();
				}
			}
			String score = itsGameBoardLogics.getScore();
			String name = JOptionPane.showInputDialog(this, "Game Over, your score is: " + score + System.getProperty("line.separator") + "please insert your name");
			HighScores.insertScore(score, name);
	}
	

	
	public static void main(String args[])
	{
		GameBoard frame = new GameBoard();
	}

	@Override
	public void keyPressed(KeyEvent e) {

		switch(e.getKeyCode())
		{
			case KeyEvent.VK_DOWN: 
				// the user pressed key down
			{
				if (this.itsGameBoardLogics.BoardDown(board, itsTileBank))
				{
					updatBoard();
				}
			}
			break;
			case KeyEvent.VK_UP: 
				// the user pressed key up
			{
				if (this.itsGameBoardLogics.BoardUp(board, itsTileBank))
				{
					updatBoard();
				}
			}
			break;
			case KeyEvent.VK_LEFT: 
				// the user pressed key left
			{
				if (this.itsGameBoardLogics.BoardLeft(board, itsTileBank));
				{
					updatBoard();
				}
			}
			break;
			case KeyEvent.VK_RIGHT: 
				// the user pressed key right
			{
				if (this.itsGameBoardLogics.BoardRight(board, itsTileBank))
				{
					updatBoard();
				}
			}
			break;
		}	 
	 }
        
	

	@Override
	public void keyReleased(KeyEvent e) {
		
	}

	@Override
	public void keyTyped(KeyEvent e) {
		
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		
	}
	 
}
