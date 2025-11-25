using System;
using System.Collections;
using System.Collections.Generic;


//flyweight легковес

// фабрика стилей 
TextStyleFactory styleFactory = new TextStyleFactory(new List<SharedStyle>()
{
    new SharedStyle("Arial", 12, "Black"),
    new SharedStyle("Times New Roman", 14, "Blue"),
    new SharedStyle("Courier New", 10, "Red")
});

styleFactory.ListFlyweights();

AddCharacterToDocument(styleFactory, "Arial", 12, "Black", 'П', 0);
AddCharacterToDocument(styleFactory, "Arial", 12, "Black", 'р', 1);
AddCharacterToDocument(styleFactory, "Times New Roman", 14, "Blue", 'З', 2);
AddCharacterToDocument(styleFactory, "Courier New", 10, "Red", 'к', 3);
AddCharacterToDocument(styleFactory, "Arial", 12, "Black", 'о', 4);

styleFactory.ListFlyweights();
}

static void AddCharacterToDocument(TextStyleFactory styleFactory,
string font, int size, string color, char symbol, int position)
    {
        Console.WriteLine();
        TextFlyweight flyweight = styleFactory.GetFlyweight(new SharedStyle(font, size, color));
        flyweight.Process(new UniqueCharacter(symbol, position));
    }


// ВНУТРЕННЕЕ СОСТОЯНИЕ 
struct SharedStyle
{
    private string font;
    private int size;
    private string color;

    public SharedStyle(string font, int size, string color)
    {
        this.font = font;
        this.size = size;
        this.color = color;
    }
    public string Font { get => font; }
    public int Size { get => size; }
    public string Color { get => color; }
}

// ВНЕШНЕЕ СОСТОЯНИЕ 
struct UniqueCharacter
{
    private char symbol;
    private int position;

    public UniqueCharacter(char symbol, int position)
    {
        this.symbol = symbol;
        this.position = position;
    }
    public char Symbol { get => symbol; }
    public int Position { get => position; }
}

// ЛЕГКОВЕС
class TextFlyweight
{
    private SharedStyle sharedStyle;

    public TextFlyweight(SharedStyle sharedStyle)
    {
        this.sharedStyle = sharedStyle;
    }

    public void Process(UniqueCharacter unique)
    {
        Console.WriteLine("Новые данные: разделяемое - " + sharedStyle.Font + " " +
                         sharedStyle.Size + " " + sharedStyle.Color +
                         " уникальное - " + unique.Symbol + " (символ) " + unique.Position + " (позиция)");
    }

    public string GetData() => sharedStyle.Font + " " + sharedStyle.Size + " " + sharedStyle.Color;
}

// ФАБРИКА ЛЕГКОВЕСОВ
class TextStyleFactory
{
    private Hashtable flyweights;
    private string GetKey(SharedStyle shared) => shared.Font + " " + shared.Size + " " + shared.Color;

    public TextStyleFactory(List<SharedStyle> shareds)
    {
        flyweights = new Hashtable();
        foreach (SharedStyle shared in shareds)
        {
            flyweights.Add(GetKey(shared), new TextFlyweight(shared));
        }
    }

    public TextFlyweight GetFlyweight(SharedStyle shared)
    {
        string key = GetKey(shared);
        if (!flyweights.Contains(key))
        {
            Console.WriteLine("Фабрика стилей: объект не найден " + key);
            flyweights.Add(key, new TextFlyweight(shared));
        }
        else
        {
            Console.WriteLine("Фабрика стилей: объект найден " + key);
        }
        return (TextFlyweight)flyweights[key]!;
    }

    public void ListFlyweights()
    {
        int count = flyweights.Count;
        Console.WriteLine("Фабрика стилей: количество : " + count);
        foreach (TextFlyweight values in flyweights.Values)
        {
            Console.WriteLine(values.GetData());
        }
    }
}
