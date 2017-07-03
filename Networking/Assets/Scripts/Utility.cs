

public static class Utility {
    //Fisher Yates Shuffle | Seed allows shuffling of array in the same order
    public static T[] ShuffleArray<T>(T[] _array, int _seed)
    {
        //Psuedo-Random Number Generator
        System.Random prng = new System.Random(_seed);

        //Iterate through array and swap Coords
        for (int i = 0; i < _array.Length - 1; i++)
        {
            //Returns a value between i(MIN) and _array.Length(MAX)
            int randIndex = prng.Next(i, _array.Length);
            T tempItem = _array[randIndex];
            _array[randIndex]= _array[i];
            _array[i] = tempItem;
        }

        return _array;
    }
}
