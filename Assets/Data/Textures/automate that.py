
file_names = ["Bedrock","Stone","Cobblestone","Stone Bricks","Cracked Stone Bricks","Chiseled Stone Bricks","Smooth Stone","Mossy Cobblestone","Mossy Stone Bricks","Granite","Polished Granite","Andesite","Polished Andesite","Diorite","Polished Diorite","Blue Ice","Dirt","Coarse Dirt","Grass Side","Grass Top","Podzol Side","Podzol Top","Mycelium Side","Mycelium Top","Path Side","Path Top","Dry Farmland","Wet Farmland","Snowed Dirt","Snowed Stone","Snow","Packed Ice","Sand","Sandstone Bottom","Sandstone Side","Carved Sandstone","Cut Sandstone","Sandstone Top","Red Sand","Red Sandstone Bottom","Red Sandstone Side","Carved Red Sandstone","Cut Red Sandstone","Red Sandstone Top","Gravel","Clay","Terracotta","Ice","Oak Log Side","Oak Log Top","Oak Planks","Oak Leaves","Spruce Log Side","Spruce Log Top","Spruce Planks","Spruce Leaves","Birch Log Side","Birch Log Top","Birch Planks","Birch Leaves","Obsidian","Crying Obsidian","Bricks","Glass","Jungle Log Side","Jungle Log Top","Jungle Planks","Jungle Leaves","Acacia Log Side","Acacia Log Top","Acacia Planks","Acacia Leaves","Dark Oak Log Side","Dark Oak Log Top","Dark Oak Planks","Dark Oak Leaves","Prismarine","Prismarine Bricks","Dark Prismarine","Sea Lantern","Stripped Oak Log Side","Stripped Oak Log Top","Stripped Spruce Log Side","Stripped Spruce Log Top","Stripped Birch Log Side","Stripped Birch Log Top","Stripped Jungle Log Side","Stripped Jungle Log Top","Stripped Acacia Log Side","Stripped Acacia Log Top","Stripped Dark Oak Log Side","Stripped Dark Oak Log Top","Bookshelf","Pumpkin Side","Pumpkin Top","Carved Pumpkin","White Wool","Light Gray Wool","Gray Wool","Black Wool","Red Wool","Orange Wool","Yellow Wool","Lime Wool","Green Wool","Light Blue Wool","Cyan Wool","Blue Wool","Pink Wool","Magenta Wool","Purple Wool","Brown Wool","White Concrete","Light Gray Concrete","Gray Concrete","Black Concrete","Red Concrete","Orange Concrete","Yellow Concrete","Lime Concrete","Green Concrete","Light Blue Concrete","Cyan Concrete","Blue Concrete","Pink Concrete","Magenta Concrete","Purple Concrete","Brown Concrete","White Terracotta","Light Gray Terracotta","Gray Terracotta","Black Terracotta","Red Terracotta","Orange Terracotta","Yellow Terracotta","Lime Terracotta","Green Terracotta","Light Blue Terracotta","Cyan Terracotta","Blue Terracotta","Pink Terracotta","Magenta Terracotta","Purple Terracotta","Brown Terracotta","White Glazed Terracotta","Light Gray Glazed Terracotta","Gray Glazed Terracotta","Black Glazed Terracotta","Red Glazed Terracotta","Orange Glazed Terracotta","Yellow Glazed Terracotta","Lime Glazed Terracotta","Green Glazed Terracotta","Light Blue Glazed Terracotta","Cyan Glazed Terracotta","Blue Glazed Terracotta","Pink Glazed Terracotta","Magenta Glazed Terracotta","Purple Glazed Terracotta","Brown Glazed Terracotta","White Stained Glass","Light Gray Stained Glass","Gray Stained Glass","Black Stained Glass","Red Stained Glass","Orange Stained Glass","Yellow Stained Glass","Lime Stained Glass","Green Stained Glass","Light Blue Stained Glass","Cyan Stained Glass","Blue Stained Glass","Pink Stained Glass","Magenta Stained Glass","Purple Stained Glass","Brown Stained Glass","Glowstone","Netherrack","Nether Quartz Ore","Nether Gold Ore","Crimson Nylium Side","Crimson Nylium Top","Warped Nylium Side","Warped Nylium Top","Nether Bricks","Carved Nether Bricks","Cracked Nether Bricks","Red Nether Bricks","Soul Sand","Soul Soil","Quartz Brick","Carved Quartz Block Top","Warped Stem Side","Warped Stem Top","Warped Planks","Warped Wart Block","Crimson Stem Side","Crimson Stem Top","Crimson Planks","Crimson Wart Block","Stripped Warped Stem Side","Stripped Warped Stem Top","Stripped Crimson Stem Side","Stripped Crimson Stem Top","Smooth Quartz Block","Quartz Block","Quartz Pillar Side","Quartz Pillar Top","Carved Quartz Side","Magma Block","Basalt Side","Basalt Top","Polished Basalt Side","Polished Basalt Top","Blackstone Side","Blackstone Top","Polished Blackstone","Blackstone Bricks","Cracked Blackstone Bricks","Carved Blackstone","Gilded Blackstone","Ancient Debris Side","Ancient Debris Top","Netherite Block","Shroomlight","End Stone","End Stone Bricks","Purpur Block","Purpur Pillar Side","Purpur Pillar Top","Cactus Bottom","Cactus Side","Cactus Top","Smithing Table Top","Coal Ore","Iron Ore","Gold Ore","Diamond Ore","Redstone Ore","Lapis Lazuli Ore","Emerald Ore","Crafting Table Side","Crafting Table Top","Furnace Front","Furnace Front On","Furnace Side","Furnace Top","TNT Side","TNT Top","TNT Bottom","Coal Block","Iron Block","Gold Block","Diamond Block","Redstone Block","Lapis Lazuli Block","Emerald Block"]
for x in range(16):
    for y in range(16):
        this_string = """%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 0}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: d54d6931cff73d64cb0572e72ebc0e5d, type: 3}}
  m_Name: Andesite
  m_EditorClassIdentifier: 
  uvs:
  - {{x: {0}, y: {1}}}
  - {{x: {0}, y: {3}}}
  - {{x: {2}, y: {3}}}
  - {{x: {2}, y: {1}}}
""".format(x/16,y/16,(x+1)/16,(y+1)/16)
        f = open(file_names[x+(y*16)]+".asset","x")
        f.write(this_string)
        f.close()
