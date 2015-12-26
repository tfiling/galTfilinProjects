

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.EventQueue;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.text.SimpleDateFormat;
import java.util.Calendar;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTable;
import javax.swing.SwingConstants;
import javax.swing.border.EmptyBorder;
import javax.swing.border.MatteBorder;
import javax.swing.table.DefaultTableModel;

public class HighScores extends JFrame {

	//fields:
	private JPanel contentPane;
	private JTable table;
	private JButton btnExit;
	private JPanel tableHeaders;
	private JLabel lblName;
	private JLabel lblScore;
	private JLabel lblDate;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					HighScores frame = new HighScores();
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
	public HighScores() {
		setResizable(false);
		setTitle("High Scores");
		setBounds(100, 100, 600, 419);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(new BorderLayout(0, 0));
		
		JPanel panel = new JPanel();
		contentPane.add(panel, BorderLayout.SOUTH);
		
		btnExit = new JButton("close");
		btnExit.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				setVisible(false);
			}
		});
		panel.add(btnExit);
		
		table = new JTable();
		table.setRowHeight(32);
		table.setModel(new DefaultTableModel(
			new Object[][] {
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
				{null, null, null},
			},
			new String[] {
				"Name", "Score", "Date"
			}
		) {
			Class[] columnTypes = new Class[] {
				String.class, Object.class, Object.class
			};
			public Class getColumnClass(int columnIndex) {
				return columnTypes[columnIndex];
			}
			boolean[] columnEditables = new boolean[] {
				false, true, true
			};
			public boolean isCellEditable(int row, int column) {
				return columnEditables[column];
			}
		});
		table.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS);
		table.setRowSelectionAllowed(false);
		table.setFont(new Font("Tahoma", Font.PLAIN, 20));
		table.setBorder(new MatteBorder(2, 2, 4, 2, (Color) new Color(0, 0, 0)));

		updateTable();
		
		contentPane.add(table, BorderLayout.CENTER);
		
		tableHeaders = new JPanel();
		contentPane.add(tableHeaders, BorderLayout.NORTH);
		tableHeaders.setLayout(new GridLayout(0, 3, 0, 0));
		
		lblName = new JLabel("Name");
		lblName.setHorizontalAlignment(SwingConstants.CENTER);
		tableHeaders.add(lblName);
		
		lblScore = new JLabel("Score");
		lblScore.setHorizontalAlignment(SwingConstants.CENTER);
		tableHeaders.add(lblScore);
		
		lblDate = new JLabel("Date");
		lblDate.setHorizontalAlignment(SwingConstants.CENTER);
		tableHeaders.add(lblDate);
	}

	public void updateTable() {
		try {
			BufferedReader br = new BufferedReader(new FileReader("records_table.txt"));
			String line = br.readLine();
			String[] splitedLine;
			for (int i = 0; i < 10 && !line.isEmpty(); i++)
			{
				splitedLine = line.split(",");
				table.getModel().setValueAt(splitedLine[0], i, 0);
				table.getModel().setValueAt(splitedLine[1], i, 1);
				table.getModel().setValueAt(splitedLine[2], i, 2);
				line = br.readLine();
			}
			
			
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}

		
	}

	public static void insertScore(String score, String name) {
		try {
			FileReader fr = new FileReader("records_table.txt");
			BufferedReader br = new BufferedReader(fr);
			String line = br.readLine();
			long thisScore = Long.parseLong(score);
			String[] splitedLine;
			String newRecordTable = "";
			boolean thisIsNewRecord = false;
			int i = 0;
			while ((line != null && i <= 9 & !thisIsNewRecord & !line.isEmpty()) |
					(line != null && i <= 8 & thisIsNewRecord & !line.isEmpty()))
			{				
				if (!thisIsNewRecord)
				{
					splitedLine = line.split(",");
					long currentScore = Long.parseLong(splitedLine[1]);
					if (thisScore >= currentScore)
					{
						Calendar cal = Calendar.getInstance();
				    	SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm");
				    	String date = sdf.format(cal.getTime());
				    	if (name == null || name.length() == 0)
				    		name = "John Doe";
						newRecordTable = newRecordTable + name + "," + score + "," + date + System.getProperty("line.separator") + line + System.getProperty("line.separator");
						thisIsNewRecord = true;
					}
					else
						newRecordTable = newRecordTable + line + System.getProperty("line.separator"); 

				}
				else if (i <= 8)
					newRecordTable = newRecordTable + line + System.getProperty("line.separator"); 

				line = br.readLine();
				i++;
			}
			if (thisIsNewRecord)
				overwriteRecordsTable(newRecordTable);
			
			
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}	
	}

	private static void overwriteRecordsTable(String newRecordTable) 
	{
	    try {
	        File recordsFile = new File("records_table.txt");
	    	if(recordsFile.exists()){
	            recordsFile.delete();
	        }
	        PrintWriter pw = new PrintWriter(new File("records_table.txt"));
	        pw.print(newRecordTable);
	        pw.close();
	    } catch (FileNotFoundException e) {
	        e.printStackTrace();  
	    }
	}

}
