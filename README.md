# Transfer Batch Processing
Description
This console application calculates the daily commissions that should be charged to each account based on outbound transfers, following these rules:
* 10% Commission: Each account is charged 10% of the total amount of each transfer made.
* Exemption for the largest transfer: The highest transfer of the day is exempt from commission.
* The program reads a CSV file with the structure <Account_ID>,<Transfer_ID>,<Total_Transfer_Amount>, processes the data, and outputs the total commissions charged per account.

## CSV File Structure
The CSV file that the program processes should follow the format:
 * <Account_ID>,<Transfer_ID>,<Total_Transfer_Amount>

## Example Input:

```
A10,T1000,100.00
A11,T1001,100.00
A10,T1002,200.00
A10,T1003,300.00
```

## Example Output:
Given the above input, the program will produce the following output:

```
A10,30.00
A11,10.00
```

# Requirements
[.NET Core 8.0 LTS SDK](https://dotnet.microsoft.com/download/dotnet-core/8.0)

# How to Build the Project
To build and run the project, follow these steps:
* Clone the project.
* Navigate to the project folder using the terminal.

Run the following command to restore dependencies and build the project:

```
dotnet build
```

# How to Run the Program
To run the console application, use the following syntax:
```
dotnet run --project .\TransferBatchProcessing.csproj <Path_to_transfers_file>
```

## Example
If you have a CSV file named transfers.csv in the current directory, run the following command:

```
dotnet run --project .\TransferBatchProcessing.csproj .\transfers.csv
```

# Project Structure
Project directory look like this:

```
TransferBatchProcessing/
│
├── TransferBatchProcessing.sln           # .NET solution file
├── TransferBatchProcessing/
│   ├── Program.cs                        # Main application entry point
│   ├── ProcessData.cs                    # Logic for processing transfers
│   └── Transfer.cs                       # Transfer model class
│
├── TransferBatchProcessingTests          # Unit tests project
│   └── ProcessData.cs                    # Unit tests using xUnit and FluentAssertions
│
├── sample_transfers.csv                  # Sample for use on input
└── README.md                             # Instructions file
```

# Unit Testing

The project includes unit tests to validate the commission calculation logic. To run the tests, use the following command:

```
dotnet test
```

The tests cover various scenarios, including:
* Valid transfer files.
* Correct commission calculation.
* Handling of non-existent or malformed files.
