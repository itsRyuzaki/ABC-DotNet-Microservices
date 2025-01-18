namespace ABC.Accessories.Constants;

public class AccessoriesType
{
    public static readonly string Mobile = "Mobile";
    public static readonly string Computer = "Computer";

    public static readonly HashSet<string> AllowedTypes = [.. (string[])
                                                                    [
                                                                        Mobile,
                                                                        Computer
                                                                    ]
                                                            ];

}