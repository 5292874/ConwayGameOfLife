using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BadLife
{
    /// <summary>
    /// Represents a state of the world in
    /// Conway's Game of Life
    /// </summary>
    public sealed class World
    {
        private const char AliveChar = '*';
        private const char DeadChar = '_';

        private BitArray[] array; // contains the current state
        private BitArray[] tempArray; // used while creating the next state during evolution
        
        private readonly int rows;
        private readonly int cols;

        public static World LoadFromString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
            using (StreamReader streamReader = new StreamReader(memoryStream))
                return new World(streamReader);
        }

        public static World LoadFromTextFile(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath))
                throw new ArgumentNullException(nameof(fullFilePath));

            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException($"Cannot find the file {fullFilePath}", fullFilePath);

            using (FileStream fileStream = File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader streamReader = new StreamReader(fileStream))
                return new World(streamReader);
        }

        public void PrintToConsole()
        {
            using (StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput()))
                PrintToStream(streamWriter);
        }

        public string AsString()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            {
                PrintToStream(streamWriter);
                memoryStream.Position = 0;

                using (StreamReader streamReader = new StreamReader(memoryStream))
                    return streamReader.ReadToEnd();
            }
        }

        public void Evolve()
        {
            for (int i = 0; i < rows; i++)
            {
                int previousRowIndex = i == 0 ? rows - 1 : i - 1;
                int nextRowIndex = i == rows - 1 ? 0 : i + 1;

                BitArray previousRow = array[previousRowIndex];
                BitArray currentRow = array[i];
                BitArray nextRow = array[nextRowIndex];

                BitArray target = tempArray[i];
                target.SetAll(false);

                for (int j = 0; j < cols; j++)
                {
                    int previousColumnIndex = j == 0 ? cols - 1 : j - 1;
                    int nextColumnIndex = j == cols - 1 ? 0 : j + 1;

                    // 1 of 3) Count the neighbors
                    int numberNeighbors = 0;

                    if (previousRow[previousColumnIndex]) numberNeighbors++;
                    if (previousRow[j]) numberNeighbors++;
                    if (previousRow[nextColumnIndex]) numberNeighbors++;
                    
                    if (currentRow[previousColumnIndex]) numberNeighbors++;
                    // Do not include the cell itself!
                    if (currentRow[nextColumnIndex]) numberNeighbors++;
                    
                    if (nextRow[previousColumnIndex]) numberNeighbors++;
                    if (nextRow[j]) numberNeighbors++;
                    if (nextRow[nextColumnIndex]) numberNeighbors++;

                    // 2 of 3) Update the state of the cells
                    // We only need to set the true values
                    if (array[i][j])
                    {
                        if (numberNeighbors == 2 || numberNeighbors == 3)
                            tempArray[i][j] = true;
                    }
                    else
                    {
                        if (numberNeighbors == 3)
                            tempArray[i][j] = true;
                    }
                }
            }

            // 3 of 3) Swap the temp data structure with the current one
            // (we do not want to allocate when we evolve)
            BitArray[] temp = array;
            array = tempArray;
            tempArray = temp;
        }

        private void PrintToStream(StreamWriter streamWriter)
        {
            for (int i = 0; i < rows; i++)
            {
                BitArray row = array[i];

                for (int j = 0; j < cols; j++)
                    streamWriter.Write(row[j] ? AliveChar : DeadChar);

                if (i != rows - 1)
                    streamWriter.WriteLine();
            }

            streamWriter.Flush();
        }

        private World(StreamReader streamReader)
        {
            List<BitArray> tempList = new List<BitArray>();

            rows = 0;
            cols = 0;
            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
                if (rows == 0)
                {
                    cols = line.Length;

                    if (cols == 0)
                        throw new Exception("Empty line");
                }
                else
                {
                    if (cols != line.Length)
                        throw new Exception("Not all the lines contain the same number of characters");
                }

                BitArray bitArray = new BitArray(cols);

                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];

                    switch (c)
                    {
                        case AliveChar:
                            bitArray[i] = true;
                            break;

                        case DeadChar:
                            break;

                        default:
                            throw new Exception($"Found unexpected character line {rows}, columns {i}: '{c}'.");
                    }
                }

                tempList.Add(bitArray);
                rows++;
            }

            if (rows < 3)
                throw new Exception($"The number of rows ({rows}) cannot be less than 3.");

            if (cols < 3)
                throw new Exception($"The number of columns ({cols}) cannot be less than 3.");

            array = tempList.ToArray();
            tempArray = new BitArray[rows];

            for (int i = 0; i < rows; i++)
                tempArray[i] = new BitArray(cols);
        }
    }
}