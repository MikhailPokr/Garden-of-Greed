namespace Garden
{
    public enum ParamType
    {
        //TreeType
        IsFruit = 1,
        IsEvil = 2,
        
        //Tree General
        LastGrowthStage = 100,
        TreeSprite = 101,
        StageTime = 102,
        MaxStage = 103,
        GreenColorOffset = 104,
        PenaltyPerPoint = 105,
        
        //Wood
        WoodCost = 200,
        DryWoodCost = 201,
        WoodColor = 202,
        
        //AutoBread
        AutoBreedCount = 300,
        AutoBreedLocation = 301,
        
        //Fruit
        FruitCount = 401,
        LastFruitStage = 402,
        CostMultiplier = 403,
        RotingTime = 404,
        GrowthChance = 405,
        FruitSprite = 406,
        FruitColor = 407,
        FruitColorOffset = 408,
        StartQuality = 408,
        FruitLifeRegeneration = 409,
        
        //Grass
        SubCell = 500,
        GrowTime = 501,
    }
}