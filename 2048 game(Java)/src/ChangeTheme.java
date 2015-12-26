import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.EventQueue;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.DefaultComboBoxModel;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSplitPane;
import javax.swing.border.EmptyBorder;


public class ChangeTheme extends JFrame {

	//fields:
	private JPanel contentPane;
	
	private Color panelColor;
	private Color textColor;
	private Color boardBackground;
	private TileBank tileBank;
	private final JLabel lblDesignPreview = new JLabel();
	private JLabel[] iconsPreview;
	public boolean themeChanged;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					ChangeTheme frame = new ChangeTheme();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 */
	public ChangeTheme() {
		setDefaultCloseOperation(JFrame.DO_NOTHING_ON_CLOSE);
		setResizable(false);
		setBounds(100, 100, 450, 300);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		contentPane.setLayout(new BorderLayout(0, 0));
		setContentPane(contentPane);
		
		JButton btnDone = new JButton("Done");
		btnDone.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				setVisible(false);
			}
		});
		contentPane.add(btnDone, BorderLayout.SOUTH);
		
		JLabel lblPleaseChooseA = new JLabel("please choose a prefered design and icons");
		contentPane.add(lblPleaseChooseA, BorderLayout.NORTH);
		
		JPanel panel = new JPanel();
		contentPane.add(panel, BorderLayout.CENTER);
		panel.setLayout(new GridLayout(0, 2, 0, 0));
		
		JSplitPane splitPane = new JSplitPane();
		panel.add(splitPane);
		splitPane.setOrientation(JSplitPane.VERTICAL_SPLIT);
		
		final JComboBox comboBox_Designs = new JComboBox();
		comboBox_Designs.setModel(new DefaultComboBoxModel(new String[] {"classic", "black mamba", "barbie's world"}));
		splitPane.setLeftComponent(comboBox_Designs);
		
		
		comboBox_Designs.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		     int i = comboBox_Designs.getSelectedIndex();
        		     switch (i)
        		     {
        		    	 case 0:
        		    			panelColor = new Color(160, 140, 120);
        		    			textColor = Color.white;
        		    			boardBackground = new Color(205, 193, 181);
        		    			lblDesignPreview.setIcon(new ImageIcon("classic.jpg"));
        		    			themeChanged = true;
        		    			pack();
        		    		 break;
        		    	 case 1:
	     		    			panelColor = Color.black;
	     		    			textColor = Color.white;
	     		    			boardBackground = new Color(120, 120, 120);
	     		    			lblDesignPreview.setIcon(new ImageIcon("blackMamba.jpg"));
        		    			themeChanged = true;
	     		    			pack();
        		    		 break;
        		    	 case 2:
        		    		 	panelColor = new Color(255, 20, 255);
	     		    			textColor = Color.white;
	     		    			boardBackground = new Color(250, 170, 250);
	     		    			lblDesignPreview.setIcon(new ImageIcon("barbiesWorld.jpg"));
        		    			themeChanged = true;
	     		    			pack();
        		    		 break;
    		    		 default:
	    		    			panelColor = new Color(160, 140, 120);
	     		    			textColor = Color.white;
	     		    			boardBackground = new Color(205, 193, 181);
	     		    			lblDesignPreview.setIcon(new ImageIcon("classic.jpg"));
        		    			themeChanged = true;
	     		    			pack();
    		    			 break;
        		     }
        		    		 
        	}
        });
		
		
		this.lblDesignPreview.setIcon(new ImageIcon("classic.jpg"));
		splitPane.setRightComponent(lblDesignPreview);
		
		JSplitPane splitPane_1 = new JSplitPane();
		panel.add(splitPane_1);
		splitPane_1.setOrientation(JSplitPane.VERTICAL_SPLIT);
		
		final JComboBox comboBox_Icons = new JComboBox();
		comboBox_Icons.setModel(new DefaultComboBoxModel(new String[] {"classic icons", "pockemon fan", "iPhone style"}));
		splitPane_1.setLeftComponent(comboBox_Icons);
		
		comboBox_Icons.addActionListener(new ActionListener() {
        	public void actionPerformed(ActionEvent arg0) {
        		     int i = comboBox_Icons.getSelectedIndex();
        		     switch (i)
        		     {
        		    	 case 0:
        		    			tileBank = new TileBank("classic");
        		    			updateIconPreview(tileBank);
        		    			pack();
        		    		 break;
        		    	 case 1:
        		    		 	tileBank = new TileBank("Pockemon");
	     		    			updateIconPreview(tileBank);
	     		    			pack();
        		    		 break;
        		    	 case 2:
	        		    		tileBank = new TileBank("iphone");
	     		    			updateIconPreview(tileBank);
	     		    			pack();
        		    		 break;
    		    		 default:
	    		    			tileBank = new TileBank("classic");
	     		    			updateIconPreview(tileBank);
	     		    			pack();
    		    			 break;
        		     }        		    		 
        	}
        });
		
		JPanel panelIconPreview = new JPanel();
		panelIconPreview.setLayout(new GridLayout(4, 4));
		splitPane_1.setRightComponent(panelIconPreview);
		
		//constructs labels for icons preview
		this.iconsPreview = new JLabel[14];
		
		this.iconsPreview[0] = new JLabel();
		panelIconPreview.add(this.iconsPreview[0]);
		
		this.iconsPreview[1] = new JLabel();
		panelIconPreview.add(this.iconsPreview[1]);
		
		this.iconsPreview[2] = new JLabel();
		panelIconPreview.add(this.iconsPreview[2]);
		
		this.iconsPreview[3] = new JLabel();
		panelIconPreview.add(this.iconsPreview[3]);
		
		this.iconsPreview[4] = new JLabel();
		panelIconPreview.add(this.iconsPreview[4]);
		
		this.iconsPreview[5] = new JLabel();
		panelIconPreview.add(this.iconsPreview[5]);
		
		this.iconsPreview[6] = new JLabel();
		panelIconPreview.add(this.iconsPreview[6]);
		
		this.iconsPreview[7] = new JLabel();
		panelIconPreview.add(this.iconsPreview[7]);
		
		this.iconsPreview[8] = new JLabel();
		panelIconPreview.add(this.iconsPreview[8]);
		
		this.iconsPreview[9] = new JLabel();
		panelIconPreview.add(this.iconsPreview[9]);
		
		this.iconsPreview[10] = new JLabel();
		panelIconPreview.add(this.iconsPreview[10]);
		
		this.iconsPreview[11] = new JLabel();
		panelIconPreview.add(this.iconsPreview[11]);
		
		this.iconsPreview[12] = new JLabel();
		panelIconPreview.add(this.iconsPreview[12]);
		
		this.iconsPreview[13] = new JLabel();
		panelIconPreview.add(this.iconsPreview[13]);
		
		
		this.panelColor = new Color(160, 140, 120);
		this.textColor = Color.white;
		this.boardBackground = new Color(205, 193, 181);
		
		this.tileBank = new TileBank("classic");
		
		updateIconPreview(tileBank);
		
		this.pack();
	}

	public Color getPanelColor() {
		return panelColor;
	}

	public Color getTextColor() {
		return textColor;
	}


	public Color getBoardBackground() {
		return boardBackground;
	}

	public TileBank getTileBank() {
		return tileBank;
	}
	
	//update the preview icons to the current chosen icons
	private final void updateIconPreview(TileBank tileBank)
	{
		this.iconsPreview[0].setIcon(tileBank.tile2.getTileIcon());
		this.iconsPreview[1].setIcon(tileBank.tile4.getTileIcon());
		this.iconsPreview[2].setIcon(tileBank.tile8.getTileIcon());
		this.iconsPreview[3].setIcon(tileBank.tile16.getTileIcon());
		this.iconsPreview[4].setIcon(tileBank.tile32.getTileIcon());
		this.iconsPreview[5].setIcon(tileBank.tile64.getTileIcon());
		this.iconsPreview[6].setIcon(tileBank.tile128.getTileIcon());
		this.iconsPreview[7].setIcon(tileBank.tile256.getTileIcon());
		this.iconsPreview[8].setIcon(tileBank.tile512.getTileIcon());
		this.iconsPreview[9].setIcon(tileBank.tile1024.getTileIcon());
		this.iconsPreview[10].setIcon(tileBank.tile2048.getTileIcon());
		this.iconsPreview[11].setIcon(tileBank.tile4096.getTileIcon());
		this.iconsPreview[12].setIcon(tileBank.tile8192.getTileIcon());
		this.iconsPreview[13].setIcon(tileBank.tile16384.getTileIcon());
		
		this.themeChanged = true;
		
		
	}




}
