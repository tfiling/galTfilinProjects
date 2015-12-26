package main;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.*;

public class Calculator {
	
	static Scanner sc = new Scanner(System.in);


	public static void main(String[] args) {
		if(args.length > 0) 
		{
			try {
				FileReader fr = new FileReader(args[1]);//WHEN THE USER INPUTS "< INPUT.TXT" LIKE IN THEIR EXAMPLES, THE ARGS ARAAY CONTAINS {"<", "INPUT.TXT"}
				sc = new Scanner(fr);
			} catch (FileNotFoundException e) {
				System.out.println("the file was not found!");
				System.exit(0);
			}
		}
		try
		{
			System.out.println("Matrix Calculator" + System.getProperty("line.separator") +
					"==========================" + System.getProperty("line.separator")  + 
					"Please select the scalar field:" + System.getProperty("line.separator")  +
					"1) Rational Or 2) Complex");
			String ans = sc.nextLine();
			boolean complex = false;
			if (ans.charAt(0) == '1' & ans.length() == 1)
			{
				complex = false;
			}
			else if (ans.charAt(0) == '2' & ans.length() == 1)
			{
				complex = true;
			}
			else 
			{
				System.out.println("invalid input inserted");
				System.exit(0);
			}
			
			System.out.println("Please select an option:" + System.getProperty("line.separator") +
								"1) Addition" + System.getProperty("line.separator")  + 
								"2) Multiplication" + System.getProperty("line.separator")  +
								"3) Solving linear equation systems" + System.getProperty("line.separator")  +
								"4) Exit");
			ans = sc.next();
			switch (ans){
			case "1": 
					if (complex)
						complexAddition();
					else
						raionalAddtion();
					break;
			case "2":
					if (complex)
						complexMul();
					else
						rationalMul();
					break;
			case "3": 
					if (complex)
						complexSolve();
					else
						rationalSolve();
					break;
			case "4":	System.exit(1);
					break;
			default: 
					System.out.println("invalid input inserted");
					System.exit(0);
					break;
			}
			sc.close();
		}
		catch(NoSuchElementException e){
			System.out.println("the inserted file was not valid!");
		}
	}

	/////////////////////////////////////////////////////////
	private static void complexAddition() {
		System.out.println("you have selected option 1" + System.getProperty("line.separator") +
							"Insert the matrix size: m,n");
		Matrix firstMat = null;
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (firstMat == null)
		{
			firstMat = getComplexMatFromUser(matSize);
		}
		Matrix secondMat = null;
		System.out.println("Insert the matrix size: m,n");
		ans = sc.next();
		matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (secondMat == null)
		{
			secondMat = getComplexMatFromUser(matSize);
		}
		Matrix result = firstMat.add(secondMat);
		if (result != null) 
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else
			System.out.println("calculation error!");
	}	

	/////////////////////////////////////////////////////////
	private static void raionalAddtion() {
		System.out.println("you have selected option 1" + System.getProperty("line.separator") +
				"Insert the matrix size: m,n");
		Matrix firstMat = null;
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (firstMat == null)
		{
			firstMat = getRationalMatFromUser(matSize);
		}
		Matrix secondMat = null;
		System.out.println("Insert the matrix size: m,n");
		ans = sc.next();
		matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (secondMat == null)
		{
			secondMat = getRationalMatFromUser(matSize);
		}
		Matrix result = firstMat.add(secondMat);
		if (result != null) 
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else
			System.out.println("calculation error!");
	}

	/////////////////////////////////////////////////////////
	private static void complexMul() {
		System.out.println("you have selected option 2" + System.getProperty("line.separator") +
				"Insert the matrix size: m,n");	
		Matrix firstMat = null;
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (firstMat == null)
		{
			firstMat = getComplexMatFromUser(matSize);
		}
		Matrix secondMat = null;
		System.out.println("Insert the matrix size: m,n");
		ans = sc.next();
		matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (secondMat == null)
		{
			secondMat = getComplexMatFromUser(matSize);
		}
		Matrix result = firstMat.mul(secondMat);
		if (result != null) 
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else
			System.out.println("calculation error!");
	}

	/////////////////////////////////////////////////////////
	private static void rationalMul() {
		System.out.println("you have selected option 2" + System.getProperty("line.separator") +
				"Insert the matrix size: m,n");
		Matrix firstMat = null;
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (firstMat == null)
		{
			firstMat = getRationalMatFromUser(matSize);
		}
		Matrix secondMat = null;
		System.out.println("Insert the matrix size: m,n");
		ans = sc.next();
		matSize = verifyInsertedSize(ans);
		System.out.println("Insert the matrix");
		while (secondMat == null)
		{
			secondMat = getRationalMatFromUser(matSize);
		}
		Matrix result = firstMat.mul(secondMat);
		if (result != null) 
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else
			System.out.println("calculation error!");
	}


	/////////////////////////////////////////////////////////
	private static void complexSolve() {
		System.out.println("you have selected option 3" + System.getProperty("line.separator") +
				"Insert the matrix size: m,n");	
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		System.out.println("Insert Matrix");	
		Matrix newMat = null;
		while (newMat == null)
		{
			newMat = getComplexMatFromUser(matSize);
		}
		Matrix result = newMat.Solve();
		if (result != null)
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else 
			System.out.println("calculation error!");

	}

	/////////////////////////////////////////////////////////
	private static void rationalSolve() {
		System.out.println("you have selected option 3" + System.getProperty("line.separator") +
				"Insert the matrix size: m,n");	
		String ans = sc.next();
		int[] matSize = verifyInsertedSize(ans);
		Matrix newMat = null;
		System.out.println("Insert the matrix");
		while (newMat == null)
		{
			newMat = getRationalMatFromUser(matSize);
		}
		Matrix result = newMat.Solve();
		if (result != null)
			System.out.println("the solution is:" + System.getProperty("line.separator") +
								result.toString());
		else 
			System.out.println("calculation error!");

	}
	

	
	private static int[] verifyInsertedSize(String ans) 
	{
		boolean valid = false;
		int m = 0,n = 0;
		while (!valid)
			{
				String[] stringSize = ans.split(",");
				if (stringSize.length == 2)
				{
					try
					{
						m = Integer.parseInt(stringSize[0]);
						n = Integer.parseInt(stringSize[1]);
						valid = (m >=1 & n >=1);
					}
					catch (NumberFormatException e)
					{
						System.out.println("please a valid matrix size: m,n");
						ans = sc.next();
					}
				}
			}
		int[] result = {m,n};
		return result;
		}

	private static Matrix getComplexMatFromUser(int[] size) {
		Matrix result = new Matrix(size[1], true);
		for (int i = 1; i <= size[0]; i++)
		{
			String stringEquasion;
			stringEquasion = sc.nextLine();
			String[] splitEquasion = stringEquasion.split("i ");
			if (splitEquasion.length != size[1])
			{
				return null;
			}
			LinkedList<Scalar> values = new LinkedList<Scalar>();
			for (int j = 0; j < size[1]; j++)
			{
				//the rational a value of a complex number in the matrix
				if (splitEquasion[j].charAt(splitEquasion[j].length() - 1) == 'i')
					splitEquasion[j] = splitEquasion[j].substring(0, splitEquasion[j].length() - 1);
				try
				{
					String s = splitEquasion[j];
					String[] splitAB = s.split("\\+");
					if (splitAB.length != 2)
					{
						return null;
					}
					String[] splitRationals = splitAB[0].split("/");
					if (splitRationals.length != 2)
					{
						return null;
					}
					int a1 = Integer.parseInt(splitRationals[0]);
					int a2 = Integer.parseInt(splitRationals[1]);
					Rational a = new Rational(a1, a2);
					//the rational b value of a complex number in the matrix
					splitRationals = splitAB[1].split("/");
					if (splitRationals.length != 2)
					{
						return null;
					}
					int b1 = Integer.parseInt(splitRationals[0]);
					int b2 = Integer.parseInt(splitRationals[1]);
					Rational b = new Rational(b1, b2);
					Complex complexNum = new Complex(a, b);
					values.add(complexNum);
				}
				catch (RuntimeException e)
				{
					return null;
				}
			}
			MathVector equasion = new MathVector(size[1], true, values);
			result.add(equasion);
		}
		return result;
	}
	
	private static Matrix getRationalMatFromUser(int[] size){ 
		Matrix result = new Matrix(size[1], false);
		for (int i = 1; i <= size[0]; i++)
		{
			String equasion;
			equasion = sc.nextLine();
			while (equasion == "")
				equasion = sc.nextLine();
			String[] splitEquasion = equasion.split(" ");
			if (splitEquasion.length != size[1])
			{
				return null;
			}
			String[] splitAB = new String[2];
			LinkedList<Scalar> values = new LinkedList<Scalar>();
			for (int j = 0; j < size[1]; j++)
			{
				try{
					splitAB = splitEquasion[j].split("/");
				}
				catch(RuntimeException e)
				{
					return null;
				}
				if (splitAB.length != 2)
				{
					return null;
				}
				int a = 0, b = 0;
				try
				{
					a = Integer.parseInt(splitAB[0]);
					b = Integer.parseInt(splitAB[1]);
				}
				catch (NumberFormatException e)
				{
					return null;
				}
				Rational num = new Rational(a, b);
				values.add(num);
			}
			MathVector eq = new MathVector(size[1], false, values);
			result.add(eq);
		}
		return result;
	}




}
