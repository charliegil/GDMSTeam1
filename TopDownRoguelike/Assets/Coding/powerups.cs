using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


	public class Program
	{
		public static void Main(string[] args)
		{
			generatePowers();
		}
		public static void generatePowers(){
		  int seed = 42;
		  Random random =  new Random();
		  
		  string [] messages = {$"The dash last for 0.{random.Next(0,100)} more more seconds",
		  $"critiqual hits multiplier increased by 1.{random.Next(0,100)}",
		  $"base attack deals 1.{random.Next(0,100)} more damage",
		  $"the transform bar gets filled {random.Next(1,10)}% faster",
		  $"the transform ability is {random.Next(5,10)}% longer",
		  $"when you have less than {random.Next(5,10)}% hp remaining, all your attacks are critiqual hits",
		  $"critiqual hits happens {random.Next(5,10)}% more often"
		  };
		  
		  for(int i= 0; i<messages.Length ; i++){
		    Console.WriteLine(messages[i]);
		  }
		}
	}
	/*
	critiqual hit multiplier increased by x%
	critiqual hits happen x% more often
	when you have less than x% hp remaining, all your attacks are critiqual hits
	the transform bar gets filled x% faster
	the transform ability is x% longuer
	when you are full health, your range attack has a better range
	when you are full health, your melee attack is x% stronger
	when you are full health, your range attack is x% stronger
	when you are full health, if you attack while dashing, it does x% more damage
	your health regeneration is increased by x unit per second
	base attack deals x%a more damage
	you get x more unit of health
	you lose x unity of health, but your speed is increased by x%
	your defense is x% stronger
	the dash ability last for x% more
	the dash ability needs x% less time to recharge
	when you dash, enemies lose sight of you
	when you dash, you take x% less damage, but you are y% slower
	each enemy you kill has a x% chnge to give you back health
	if an enemy attacks you, there is slight chance it does nothing
	if an enemy attacks you, x% of damage gets applied to him
	you can redistribute your skill points, at the expense of 1 skill point
	enemies are x% faster, but they do y% less damage
	enemies are x% slower, but they do y% more damage
	when each level starts, you get back to full health
	when each level starts, if you are full health, you get x temporary unit of health
	
	
	
	
	
	*/
