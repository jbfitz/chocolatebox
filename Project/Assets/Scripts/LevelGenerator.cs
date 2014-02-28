﻿using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public int floorCount;
	public Vector2 levelSize;
	public Vector2 baseBlockSize;
	public SpriteRenderer groundBlock;
	public bool openLevel;

	//TODO this probably shouldn't be a constant
	public const int NUMSECTIONS = 15; //Number of sections to divide map into
	//private bool done = false;

	public void Start () {
		//A grid of sections, but we might not even need to store this
        // JBF: We should store it. Ints are easier to scan than a set of almost identical GameObjects
		int[,][,] master = new int[NUMSECTIONS,floorCount][,];

		SectionBuilder lastSection = null;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				EntrancePositions entrances;
				if (lastSection == null) 
				{
					entrances = new EntrancePositions(1,-1,-1,-1);
				}
				else
				{
					entrances = new EntrancePositions(lastSection.finalEntrancePositions.eastEntrance, -1, -1, -1);
				}
				SectionBuilder newSection = new SectionBuilder(levelSize/NUMSECTIONS, this, entrances);
				int[,] section = newSection.Build();
				//Store each section in master
				master[width, height] = section;

				//generate the section
				for (int i = 0; i < section.GetLength(0); i++)
				{
					for (int j = 0; j < section.GetLength(1); j++)
					{
						if (section[i,j] == (int) AssetTypeKey.GroundBlock)
						{
							float centerX = groundBlock.sprite.bounds.extents.x  * 2 * i + (
								groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(0)* width);
							float centerY = groundBlock.sprite.bounds.extents.y * 2 * j + (
								groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(1) * height);
							Instantiate(groundBlock, new Vector3(centerX,centerY,0), new Quaternion());
                            
							//Debug.Log (centerX + ", " + centerY);
						}
					}
				}

				lastSection = newSection;
			}
		}
	}
	
	public void Update () {

	}

	public enum AssetTypeKey 
	{
		None = 0,
		GroundBlock = 1,
		Entrance = 2
	}
}
