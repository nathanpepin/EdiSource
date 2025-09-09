//TODO: Change to manual
// using Spectre.Console;
// using Spectre.Console.Advanced;
// using Spectre.Console.Rendering;
//
// namespace EdiSource.Domain.Validation.Data;
//
// public static class ValidationMessageTable
// {
//     public static Task ValidationsTableToFile(FileInfo fileInfo, EdiValidationResult validationResult, CancellationToken cancellationToken = default, params ValidationSeverity[] severities)
//     {
//         var messages = validationResult
//             .ValidationMessages
//             .Where(x => severities.Length == 0 || severities.Contains(x.Severity));
//
//         return ValidationsTableToFile(fileInfo, messages, cancellationToken);
//     }
//
//     public static Task ValidationsTableToFile(FileInfo fileInfo, IEnumerable<ValidationMessage> messages, CancellationToken cancellationToken = default)
//     {
//         var table = GenerateTable([..messages], false);
//         var console = AnsiConsole.Create(new AnsiConsoleSettings());
//         var text = console.ToAnsi(table);
//         return File.WriteAllTextAsync(fileInfo.FullName, text, cancellationToken);
//     }
//
//     public static string ValidationsTableToString(EdiValidationResult validationResult, bool includeColors = true, params ValidationSeverity[] severities)
//     {
//         var messages = validationResult
//             .ValidationMessages
//             .Where(x => severities.Length == 0 || severities.Contains(x.Severity));
//
//         return ValidationsTableToString(messages, includeColors);
//     }
//
//     public static string ValidationsTableToString(IEnumerable<ValidationMessage> messages, bool includeColors = true)
//     {
//         var table = GenerateTable([..messages], includeColors);
//         var console = AnsiConsole.Create(new AnsiConsoleSettings());
//         return console.ToAnsi(table);
//     }
//
//     public static void PrintValidations(EdiValidationResult validationResult, params ValidationSeverity[] severities)
//     {
//         var messages = validationResult
//             .ValidationMessages
//             .Where(x => severities.Length == 0 || severities.Contains(x.Severity));
//
//         PrintValidations([..messages]);
//     }
//
//     public static void PrintValidations(IEnumerable<ValidationMessage> messages)
//     {
//         AnsiConsole.Write(GenerateTable([..messages], true));
//     }
//
//     public static Table GenerateTable(ValidationMessage[] messages, bool includeColors)
//     {
//         var table = new Table();
//
//         table.AddColumns("Severity", "Subject", "Loop", "Loop Line", "Segment Line", "DE", "CE", "Value", "Message", "Segment");
//
//         foreach (var v in messages)
//         {
//             var severity = !includeColors
//                 ? v.Severity.ToString()
//                 : v.Severity switch
//                 {
//                     ValidationSeverity.None => "None",
//                     ValidationSeverity.Info => "[aqua]Info[/]",
//                     ValidationSeverity.Warning => "[yellow]Warning[/]",
//                     ValidationSeverity.Error => "[red]Error[/]",
//                     ValidationSeverity.Critical => "[fuchsia]Critical[/]",
//                     _ => throw new ArgumentOutOfRangeException(nameof(messages))
//                 };
//
//             table.AddRow(
//                 severity,
//                 v.Subject.ToString(),
//                 v.Loop ?? string.Empty,
//                 v.LoopLine?.ToString() ?? string.Empty,
//                 v.SegmentLine?.ToString() ?? string.Empty,
//                 v.DataElement?.ToString() ?? string.Empty,
//                 v.CompositeElement?.ToString() ?? string.Empty,
//                 v.Value ?? string.Empty,
//                 v.Message,
//                 v.Segment ?? string.Empty);
//         }
//
//         return table;
//     }
// }