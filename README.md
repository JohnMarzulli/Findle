# Findle

A tool to help solve that pesky daily word puzzle

Written due to too much coffee, too late in the day, with too much free time... and too little sleep.

## Using This

To run, you will need the .Net 5 SDK.

To build:

`dotnet build Findle.csproj`

## Examples

### Get A List Of Starter Words

```bash
./Findle ./words.txt
```

### Get A List Of Words With Letters In Specific Locations

Returns a list of words where 'b' is the first letter, 'a' is the third letter, and 's' is the fourth letter.

The dots in the 2nd and 5th position mean that those letters could be anything.

```bash
./Findle ./words.txt -a "b.as."
```

### Get A List Of Words That DO NOT Have Eliminated Letters

Words with 'o', 'u', and 'i' are removed.

```bash
./Findle ./words.txt -e "oui"
```

### Get A List Of Words That MUST Have Specific Letters

Words that **DO NOT** have 'q' and 'y' are removed.

```bash
./Findle ./words.txt -r "qy"
```

### Get A List Of Words That Do Not Have Eliminated Letters, Do Contain Required Letters, And Have Letters In Specific Locations

Returns a list of words that

- MUST contain 'l' and 'o' (any position)
- DO NOT contain 'h', 'a', 'p', 'f', 'i', 'e', or 'r'
- MUST contain the letter 'l' in the 4th position.

```bash
./Findle ./words.txt -r "lo" -e "hapyfier" -a "...l."
```
