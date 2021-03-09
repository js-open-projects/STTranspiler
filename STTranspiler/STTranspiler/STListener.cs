using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STTranspiler
{
    public class STListener : STBaseListener
    {
        public string programName;
        public STParser parser;
        public StringBuilder content;
        public StringBuilder externalContent;
        public StringBuilder classExternalContent;
        public List<string> globalVariables;
        public List<string> globalVariablesNames;

        public STListener(STParser _parser)
        {
            programName = "";
            parser = _parser;
            content = new StringBuilder();
            externalContent = new StringBuilder();
            classExternalContent = new StringBuilder();
            globalVariables = new List<string>();
            globalVariablesNames = new List<string>();
        }

        public override void EnterStProgram([NotNull] STParser.StProgramContext context)
        {
            if (context.program() != null)
            {
                string program = ConvertProgramToString(context.program());
                content.Append(program);
            }

            if (context.outerOperations() != null && context.outerOperations().Any())
            {
                foreach (var outerOperations in context.outerOperations())
                {
                    string operations = ConvertOuterOperationsToString(outerOperations);
                    content.AppendLine(operations);
                }
            }

            content.AppendLine(classExternalContent.ToString());
            content.AppendLine("\n}\n}");
        }

        private string ConvertProgramToString(STParser.ProgramContext context)
        {
            string program = "";
            programName = context.programName().ID().ToString();

            var a = context.GetText();

            program += "namespace " + programName + "\n{";
            program += "\npublic class " + programName + "\n{";
            program += "private static void Main(string[] args)" + "\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operations in context.operations())
                {
                    program += ConvertOperationsToString(operations);
                }
            }

            program += "\n\n {GLOBAL_VARIABLES} \nSystem.Console.ReadKey(); \n}";

            return program;
        }

        private string ConvertNamespaceOperationsToString(STParser.NamespaceOperationsContext context)
        {
            if (context.outerOperations() != null)
            {
                return ConvertOuterOperationsToString(context.outerOperations());
            }

            if (context.operations() != null)
            {
                return ConvertOperationsToString(context.operations());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertOperationsToString(STParser.OperationsContext context)
        {
            if (context.variables() != null)
            {
                return ConvertVariablesToString(context.variables());
            }

            if (context.variableAssign() != null)
            {
                return ConvertVariableAssignToString(context.variableAssign());
            }

            if (context.bracketedAssignOperation() != null)
            {
                return ConvertBracketedAssignOperationToString(context.bracketedAssignOperation());
            }

            if (context.assignOperation() != null)
            {
                return ConvertAssignOperationToString(context.assignOperation());
            }

            if (context.ifExpression() != null)
            {
                return ConvertIfExpressionToString(context.ifExpression());
            }

            if (context.caseExpression() != null)
            {
                return ConvertCaseExpressionToString(context.caseExpression());
            }

            if (context.forLoop() != null)
            {
                return ConvertForLoopToString(context.forLoop());
            }

            if (context.whileLoop() != null)
            {
                return ConvertWhileLoopToString(context.whileLoop());
            }

            if (context.repeatLoop() != null)
            {
                return ConvertRepeatLoopToString(context.repeatLoop());
            }

            if (context.EXIT() != null)
            {
                return "break;";
            }

            if (context.RETURN() != null)
            {
                return "return;";
            }

            if (context.structAssign() != null)
            {
                return ConvertStructAssignToString(context.structAssign());
            }

            if (context.functionCall() != null)
            {
                return ConvertFunctionCallToString(context.functionCall());
            }

            if (context.method() != null)
            {
                return ConvertMethodToString(context.method());
            }

            if (context.classPropertyAssign() != null)
            {
                return ConvertClassPropertyAssignToString(context.classPropertyAssign());
            }

            if (context.classMethodCall() != null)
            {
                return ConvertClassMethodCallToString(context.classMethodCall());
            }

            if (context.classObjectMethodCall() != null)
            {
                return ConvertClassObjectMethodCallToString(context.classObjectMethodCall());
            }

            if (context.parentClassMethodCall() != null)
            {
                return ConvertParentClassMethodCallToString(context.parentClassMethodCall());
            }

            if (context.callInheritedClass() != null)
            {
                return ConvertCallInheritedClassToString(context.callInheritedClass());
            }

            if (context.namespaceAccess() != null)
            {
                return ConvertNamespaceAccessToString(context.namespaceAccess());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertOuterOperationsToString(STParser.OuterOperationsContext context)
        {
            if (context.classDeclaration() != null)
            {
                return ConvertClassDeclarationToString(context.classDeclaration());
            }

            if (context.interfaceDeclaration() != null)
            {
                return ConvertInterfaceDeclarationToString(context.interfaceDeclaration());
            }

            if (context.types() != null)
            {
                return ConvertTypesToString(context.types());
            }

            if (context.@namespace() != null)
            {
                return ConvertNamespaceToString(context.@namespace());
            }

            if (context.function() != null)
            {
                return ConvertFunctionToString(context.function());
            }

            if (context.functionBlock() != null)
            {
                return ConvertFunctionBlockToString(context.functionBlock());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertStructAssignToString(STParser.StructAssignContext context)
        {
            string structAssign = "";

            if (!string.IsNullOrEmpty(structAssign))
            {
                return structAssign;
            }
            else
            {
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
            }
        }

        private string ConvertFunctionCallToString(STParser.FunctionCallContext context)
        {
            string functionCall = "";

            functionCall += $"{context.ID().ToString()} (";

            if (context.functionValue() != null && context.functionValue().Any())
            {
                functionCall += string.Join(", ", context.functionValue().Select(x => ConvertFunctionValueToString(x)));
            }

            functionCall += ");";

            return functionCall;
        }

        private string ConvertFunctionValueToString(STParser.FunctionValueContext context)
        {
            if (context.variableAssign() != null)
            {
                return ConvertVariableAssignToString(context.variableAssign());
            }

            if (context.assignValue() != null)
            {
                return ConvertAssignValueToString(context.assignValue());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertCallInheritedClassToString(STParser.CallInheritedClassContext context)
        {
            string inheritedClass = "";

            if (!string.IsNullOrEmpty(inheritedClass))
            {
                return inheritedClass;
            }
            else
            {
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
            }
        }

        private string ConvertNamespaceAccessToString(STParser.NamespaceAccessContext context)
        {
            string namespaceAccess = "";

            namespaceAccess += string.Join(".", context.ID().Select(x => x.ToString()));

            return namespaceAccess;
        }

        private string ConvertClassDeclarationToString(STParser.ClassDeclarationContext context)
        {
            string classDeclaration = "public class " + context.className().ID();
            bool colonUsed = false;

            if (context.inheritance() != null)
            {
                colonUsed = true;
                classDeclaration += ": " + context.inheritance().className().ID();
            }

            if (context.interfaceImplementation() != null)
            {
                classDeclaration += colonUsed ? ", " : ": ";
                classDeclaration += ConvertInterfaceImplementationToString(context.interfaceImplementation());
            }

            classDeclaration += "\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operations in context.operations())
                {
                    classDeclaration += ConvertOperationsToString(operations);
                }
            }

            classDeclaration += "\n}";

            return classDeclaration;
        }

        private string ConvertInterfaceDeclarationToString(STParser.InterfaceDeclarationContext context)
        {
            string interfaceDeclaration = "";

            interfaceDeclaration += $"public interface {context.interfaceName().ID()}" + "\n{";

            if (context.interfaceMethodDeclaration() != null && context.interfaceMethodDeclaration().Any())
            {
                foreach (var method in context.interfaceMethodDeclaration())
                {
                    interfaceDeclaration += ConvertInterfaceMethodDeclarationToString(method);
                }
            }

            interfaceDeclaration += "\n}";

            return interfaceDeclaration;
        }

        private string ConvertNamespaceToString(STParser.NamespaceContext context)
        {
            string @namespace = "";

            @namespace += "namespace " + context.namespaceName().ID().ToString() + "\n{";

            if (context.namespaceOperations() != null && context.namespaceOperations().Any())
            {
                foreach (var namespaceOperations in context.namespaceOperations())
                {
                    @namespace += ConvertNamespaceOperationsToString(namespaceOperations);
                }
            }

            @namespace += "}\n}";

            return @namespace;
        }

        private string ConvertVariablesToString(STParser.VariablesContext context)
        {
            string variables = "";

            if (context.variableDeclaration() != null && context.variableDeclaration().Any())
            {
                bool isGlobal = context.variableKeywords().VAR_GLOBAL() != null;

                foreach (var variableDeclaration in context.variableDeclaration())
                {
                    if (isGlobal)
                    {
                        string variable = ConvertVariableDeclarationToString(variableDeclaration, isGlobal: true);

                        if (variable != null)
                        {
                            globalVariables.Add(variable);
                        }
                    }
                    else
                    {
                        variables += ConvertVariableDeclarationToString(variableDeclaration) + "\n";
                    }
                }
            }

            return variables;
        }

        private string ConvertVariableDeclarationToString(STParser.VariableDeclarationContext context, bool omitVariableInitialization = false, bool isGlobal = false)
        {
            string line = "";
            string type = "";

            if (isGlobal)
            {
                line += "public static ";
            }

            if (context.variableType()?.dataType() != null)
            {
                type = ConvertSTDataTypeToNetDataType(context.variableType().dataType());
                line += type + " ";
            }

            foreach (var variable in context.variableName())
            {
                var enumerator = context.variableName().ToList().IndexOf(variable);

                if (variable.ID() != null)
                {
                    if (isGlobal)
                    {
                        if (globalVariablesNames.Contains(variable.ID().ToString()))
                            return null;

                        globalVariablesNames.Add(variable.ID().ToString());
                    }

                    line += variable.ID();
                }

                if (context.variableName().Any() && context.variableName().Count() > enumerator + 1)
                {
                    line += ",";
                }
            }

            if (context.lengthDeclaration() != null && type != "string")
            {
                foreach (var lengthDeclaration in context.lengthDeclaration())
                {
                    line += ConvertLengthDeclarationToString(lengthDeclaration);
                }
            }

            if (isGlobal)
            {
                line += "{ get; set; }";
            }

            if (!omitVariableInitialization && context.assignment() != null && context.assignment().ASSIGN_OPERATOR() != null && context.assignment().assignValue() != null)
            {
                line += "=" + ConvertAssignValueToString(context.assignment().assignValue()) + ";";
            }
            else if (!isGlobal)
            {
                line += ";";
            }

            return line;
        }

        private string ConvertVariableAssignToString(STParser.VariableAssignContext context, bool withoutNewLine = false)
        {
            string line = "";

            if (context.value() != null && context.value().Any())
            {
                line += ConvertValueToString(context.value()[0]);
            }

            string value = ConvertAssignValueToString(context.assignValue());

            if (string.IsNullOrEmpty(value))
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";

            line += " = " + value + ";";

            if (!withoutNewLine)
            {
                line += "\n";
            }

            return line;
        }

        private string ConvertTypesToString(STParser.TypesContext context)
        {
            string types = "";

            if (context.typeDeclaration() != null && context.typeDeclaration().Any())
            {
                foreach (var typeDeclaration in context.typeDeclaration())
                {
                    types += ConvertTypeDeclarationToString(typeDeclaration);
                }
            }

            return types;
        }

        private string ConvertInterfaceMethodDeclarationToString(STParser.InterfaceMethodDeclarationContext context)
        {
            string methodDeclaration = "";
            if (context.dataType() != null)
            {
                methodDeclaration += ConvertSTDataTypeToNetDataType(context.dataType()) + " ";
            }
            else if (context.ID() != null)
            {
                methodDeclaration += context.ID().ToString() + " ";
            }
            else
            {
                methodDeclaration += "void ";
            }

            methodDeclaration += context.interfaceMethodName().ID() + "();";

            return methodDeclaration;
        }

        private string ConvertClassPropertyAccessToString(STParser.ClassPropertyAccessContext context)
        {
            string line = $"this.{context.ID().ToString()}";

            return line;
        }

        private string ConvertClassPropertyAssignToString(STParser.ClassPropertyAssignContext context)
        {
            string line = "";

            line += $"{ConvertClassPropertyAccessToString(context.classPropertyAccess())} = {ConvertAssignValueToString(context.assignValue())};";

            return line;
        }

        private string ConvertMethodToString(STParser.MethodContext context)
        {
            string method = "";

            if (context.qualifiers() != null && context.qualifiers().Any())
            {
                method += string.Join(" ", context.qualifiers().ToList().Select(x => ConvertQualifiersToString(x)));
            }

            if (context.dataType() != null)
            {
                method += " " + ConvertSTDataTypeToNetDataType(context.dataType()) + " ";
            }
            else if (context.ID() != null && context.ID().Count() > 1 && context.ID()[1] != null)
            {
                method += " " + context.ID()[1].ToString() + " ";
            }
            else
            {
                method += " void ";
            }

            method += context.ID()[0].ToString();
            method += "()";
            method += "\n{";

            if (context.operations() != null)
            {
                foreach (var operation in context.operations())
                {
                    method += ConvertOperationsToString(operation) + ";";
                }
            }

            method += "\n}";

            return method;
        }

        private string ConvertClassMethodCallToString(STParser.ClassMethodCallContext context)
        {
            string line = "";

            line += ConvertClassPropertyAccessToString(context.classPropertyAccess()) + "(";

            if (context.functionValue() != null && context.functionValue().Any())
            {
                line += string.Join(", ", context.functionValue().Select(x => ConvertFunctionValueToString(x)));
            }

            line += ");";

            return line;
        }

        private string ConvertParentClassMethodCallToString(STParser.ParentClassMethodCallContext context)
        {
            string line = "";

            line += "base." + context.ID().ToString() + "(";

            if (context.functionValue() != null && context.functionValue().Any())
            {
                line += string.Join(", ", context.functionValue().Select(x => ConvertFunctionValueToString(x)));
            }

            line += ");";

            return line;
        }

        private string ConvertClassObjectMethodCallToString(STParser.ClassObjectMethodCallContext context)
        {
            string line = "";

            line += string.Join(".", context.ID().Select(x => x.ToString())) + "(";

            if (context.functionValue() != null && context.functionValue().Any())
            {
                line += string.Join(", ", context.functionValue().Select(x => ConvertFunctionValueToString(x)));
            }

            line += ");";

            return line;
        }

        private string ConvertQualifiersToString(STParser.QualifiersContext context)
        {
            string line = "";

            if (context.PROTECTED() != null)
            {
                line += "protected";
            }

            if (context.PUBLIC() != null)
            {
                line += "public";
            }

            if (context.PRIVATE() != null)
            {
                line += "private";
            }

            if (context.INTERNAL() != null)
            {
                line += "internal";
            }

            if (context.CONSTANT() != null)
            {
                line += "const";
            }

            if (context.OVERRIDE() != null)
            {
                line += "override";
            }

            if (!string.IsNullOrEmpty(line))
            {
                return line;
            }
            else
            {
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
            }
        }

        private string ConvertInterfaceImplementationToString(STParser.InterfaceImplementationContext context)
        {
            string line = "";

            if (context.ID() != null && context.ID().Any())
            {
                line += string.Join(", ", context.ID().ToList());
            }

            if (!string.IsNullOrEmpty(line))
            {
                return line;
            }
            else
            {
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
            }
        }

        private string ConvertTypeDeclarationToString(STParser.TypeDeclarationContext context)
        {
            if (context.customDataType() != null)
            {
                string line = "";

                if (context.customDataType().enumeratedDataType() != null)
                {
                    line = "public enum " + context.ID() + "\n" + ConvertEnumeratedDataTypeToString(context.customDataType().enumeratedDataType());
                }
                else if (context.customDataType().structuredDataType() != null)
                {
                    line = "public struct " + context.ID() + "\n" + ConvertStructuredDataTypeToString(context.customDataType().structuredDataType());
                }

                externalContent.AppendLine(line);
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertEnumeratedDataTypeToString(STParser.EnumeratedDataTypeContext context)
        {
            string line = "{\n";

            foreach (var enumItem in context.ID())
            {
                int enumerator = context.ID().ToList().FindIndex(x => x == enumItem);

                line += enumItem;
                if (context.ASSIGN_OPERATOR() != null && context.ASSIGN_OPERATOR().Any() && context.ASSIGN_OPERATOR()[enumerator] != null)
                {
                    line += "=" + ConvertAssignValueToString(context.assignValue()[enumerator]);
                }

                line += ",\n";
            }

            line += "}\n";

            return line;
        }

        private string ConvertStructuredDataTypeToString(STParser.StructuredDataTypeContext context)
        {
            string structuredDataType = "{\n";

            if (context.variableDeclaration() != null && context.variableDeclaration().Any())
            {
                foreach (var variableDeclaration in context.variableDeclaration())
                {
                    structuredDataType += "public " + ConvertVariableDeclarationToString(variableDeclaration, true) + "\n";
                }
            }

            structuredDataType += "}\n";

            return structuredDataType;
        }

        private string ConvertIfExpressionToString(STParser.IfExpressionContext context)
        {
            string line = "";

            line += ConvertIfStatement(context.ifStatement());

            if (context.elsifStatement() != null && context.elsifStatement().Any())
            {
                foreach (var statement in context.elsifStatement())
                {
                    line += ConvertElsifStatementToString(statement);
                }
            }

            if (context.elseStatement() != null)
            {
                line += ConvertElseStatementToString(context.elseStatement());
            }

            return line;
        }

        private string ConvertFunctionToString([NotNull] STParser.FunctionContext context)
        {
            string body = "";

            if (context.INTERNAL() != null)
            {
                body += "internal ";
            }
            else
            {
                body += "public ";
            }

            if (context.dataType() != null)
            {
                body += ConvertSTDataTypeToNetDataType(context.dataType()) + " ";
            }
            else if (context.ID() != null && context.ID().Count() > 1 && context.ID()[1] != null)
            {
                body += context.ID()[1].ToString() + " ";
            }
            else
            {
                body += "void ";
            }

            body += context.ID()[0].ToString() + "()\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operation in context.operations())
                {
                    body += ConvertOperationsToString(operation);
                }
            }
            body += "\n}";
            classExternalContent.AppendLine(body);

            return "";
        }

        private string ConvertExpressionToString(STParser.ExpressionContext context)
        {
            string expression = "";

            if (context.arithmeticOperator()?.EXPONENT_OPERATOR() != null)
            {
                string value1 = globalVariablesNames.Contains(context.ID()[0].ToString()) ? "GLOBAL_VARIABLES." + context.ID()[0].ToString() : context.ID()[0].ToString();
                string value2 = globalVariablesNames.Contains(context.ID()[1].ToString()) ? "GLOBAL_VARIABLES." + context.ID()[1].ToString() : context.ID()[1].ToString();
                expression += $"System.Math.Pow({value1}, {value2})";
            }
            else
            {
                expression += (globalVariablesNames.Contains(context.ID()[0].ToString()) ?
                        "GLOBAL_VARIABLES." + context.ID()[0].ToString()
                        : context.ID()[0].ToString())
                    + ConvertArithmeticOperatorToString(context.arithmeticOperator())
                    + (globalVariablesNames.Contains(context.ID()[1].ToString()) ?
                        "GLOBAL_VARIABLES." + context.ID()[1].ToString()
                        : context.ID()[1].ToString());
            }

            return expression;
        }

        private string ConvertForLoopToString(STParser.ForLoopContext context)
        {
            string value = ConvertValueToString(context.variableAssign().value()[0]);
            string stopValue = ConvertNumberValueToString(context.numberValue());
            string checkStopValue = $"({stopValue} > 0 ? {stopValue} + 1 : {stopValue} - 1 )";
            string forLoop = "for(";

            forLoop += ConvertVariableAssignToString(context.variableAssign(), true);
            forLoop += "" + value + " != " + checkStopValue + ";";

            if (context.BY() != null && context.NUMBER_VALUE() != null)
            {
                forLoop += $"{value} = {value} + {context.NUMBER_VALUE().ToString()}";
            }

            forLoop += ")";

            if (context.DO() != null && context.operations() != null && context.operations().Any())
            {
                forLoop += "\n{";
                foreach (var operations in context.operations())
                {
                    forLoop += ConvertOperationsToString(operations);
                }
                forLoop += "\n}";
            }
            else
            {
                forLoop += ";";
            }

            return forLoop;
        }

        private string ConvertWhileLoopToString(STParser.WhileLoopContext context)
        {
            string whileLoop = "while(";

            if (context.normalBracketedOperation() != null && context.normalBracketedOperation().Any())
            {
                foreach (var operation in context.normalBracketedOperation())
                {
                    whileLoop += ConvertNormalBracketedOperationToString(operation);
                }
            }

            whileLoop += ")";

            if (context.operations() != null && context.operations().Any())
            {
                whileLoop += "\n{";
                foreach (var operations in context.operations())
                {
                    whileLoop += ConvertOperationsToString(operations);
                }
                whileLoop += "\n}";
            }
            else
            {
                whileLoop += ";";
            }

            return whileLoop;
        }

        private string ConvertRepeatLoopToString([NotNull] STParser.RepeatLoopContext context)
        {
            string repeatLoop = "";

            repeatLoop += "do\n";
            repeatLoop += "{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operations in context.operations())
                {
                    repeatLoop += ConvertOperationsToString(operations);
                }
            }

            repeatLoop += "\n} while(!(";

            if (context.normalBracketedOperation() != null && context.normalBracketedOperation().Any())
            {
                foreach (var operation in context.normalBracketedOperation())
                {
                    repeatLoop += ConvertNormalBracketedOperationToString(operation);
                }
            }
            else
            {
                repeatLoop += "1";
            }

            repeatLoop += "));";

            return repeatLoop;
        }

        private string ConvertFunctionBlockToString(STParser.FunctionBlockContext context)
        {
            string functionBlock = "public class " + context.ID();

            if (context.inheritance() != null)
            {
                functionBlock += ": " + context.inheritance().className().ID();
            }

            functionBlock += "\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operations in context.operations())
                {
                    functionBlock += ConvertOperationsToString(operations);
                }
            }

            functionBlock += "\n}";

            return functionBlock;
        }

        private string ConvertCaseExpressionToString(STParser.CaseExpressionContext context)
        {
            string switchCase = "";

            switchCase = $"switch ({ConvertValueToString(context.value())})\n" + "{";

            foreach (var caseOption in context.caseOption())
            {
                switchCase += ConvertCaseOptionToString(caseOption);
            }

            if (context.ELSE() != null)
            {
                switchCase += "default: ";

                foreach (var operation in context.operations())
                {
                    switchCase += ConvertOperationsToString(operation);
                }

                switchCase += "\nbreak;";
            }

            switchCase += "\n}";

            return switchCase;
        }

        private string ConvertCaseOptionToString(STParser.CaseOptionContext context)
        {
            string caseOption = "";

            foreach (var value in context.value())
            {
                caseOption += $"case {ConvertValueToString(value)}:\n";
            }

            foreach (var operation in context.operations())
            {
                caseOption += ConvertOperationsToString(operation);
            }

            caseOption += "\nbreak;";

            return caseOption;
        }

        private string ConvertIfStatement(STParser.IfStatementContext context)
        {
            string line = "";

            line += "if(";

            foreach (var operation in context.normalBracketedOperation())
            {
                line += ConvertNormalBracketedOperationToString(operation);
            }

            line += ")\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operation in context.operations())
                {
                    line += ConvertOperationsToString(operation);
                }
            }

            line += "}";

            return line;
        }

        private string ConvertElsifStatementToString(STParser.ElsifStatementContext context)
        {
            string line = "";

            line += "else if(";

            foreach (var operation in context.normalBracketedOperation())
            {
                line += ConvertNormalBracketedOperationToString(operation);
            }

            line += ")\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operation in context.operations())
                {
                    line += ConvertOperationsToString(operation);
                }
            }

            line += "}";

            return line;
        }

        private string ConvertElseStatementToString(STParser.ElseStatementContext context)
        {
            string line = "";

            line += "else\n{";

            if (context.operations() != null && context.operations().Any())
            {
                foreach (var operation in context.operations())
                {
                    line += ConvertOperationsToString(operation);
                }
            }

            line += "}";

            return line;
        }

        private string ConvertNormalBracketedOperationToString(STParser.NormalBracketedOperationContext context)
        {
            if (context.operation() != null)
            {
                return ConvertOperationToString(context.operation());
            }

            if (context.bracketedOperation() != null)
            {
                return ConvertBracketedOperationToString(context.bracketedOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertIdWithLengthToString(STParser.IdWithLengthContext context)
        {
            string line = context.ID().ToString();

            foreach (var item in context.lengthDeclaration())
            {
                line += ConvertLengthDeclarationToString(item);
            }

            return line;
        }

        private string ConvertVariableChoiceToString(STParser.VariableChoiceContext context)
        {
            string line = "";

            for (int i = 1; i < context.ID().Length; i++)
            {
                line += ConvertLogicalOperatorToString(context.logicalOperator()[i - 1]);
                line += context.ID()[i].ToString();
            }

            return line;
        }

        private string ConvertLengthDeclarationToString(STParser.LengthDeclarationContext context)
        {
            string lengthDeclaration = "[";

            foreach (var item in context.lengthDeclarationValue())
            {
                if (item.numberValue() != null)
                {
                    lengthDeclaration += ConvertNumberValueToString(item.numberValue());
                }
                else if (item.ID() != null)
                {
                    lengthDeclaration += item.ID().ToString();
                }
            }

            lengthDeclaration += "]";

            return lengthDeclaration;
        }

        private string ConvertValueWithQuantityToString(STParser.ValueWithQuantityContext context)
        {
            string valueWithQuantity = "";

            if (!string.IsNullOrEmpty(valueWithQuantity))
            {
                return valueWithQuantity;
            }
            else
            {
                return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
            }
        }

        private string ConvertArrayValueToString(STParser.ArrayValueContext context)
        {
            string arrayValue = "[";

            foreach (var item in context.arrayValueValue())
            {
                if (item.NUMBER_VALUE() != null)
                {
                    arrayValue += item.NUMBER_VALUE().ToString();
                }
                else if (item.valueWithQuantity() != null)
                {
                    arrayValue += ConvertValueWithQuantityToString(item.valueWithQuantity());
                }
            }

            arrayValue += "]";

            return arrayValue;
        }

        private string ConvertSTDataTypeToNetDataType(STParser.DataTypeContext context)
        {
            if (context.SINT() != null)
            {
                return "sbyte";
            }

            if (context.INT() != null)
            {
                return "short";
            }

            if (context.DINT() != null)
            {
                return "int";
            }

            if (context.LINT() != null)
            {
                return "long";
            }

            if (context.USINT() != null)
            {
                return "byte";
            }

            if (context.UINT() != null)
            {
                return "ushort";
            }

            if (context.LDINT() != null)
            {
                return "uint";
            }

            if (context.ULINT() != null)
            {
                return "ulong";
            }

            if (context.REAL() != null)
            {
                return "float";
            }

            if (context.LREAL() != null)
            {
                return "double";
            }

            if (context.TIME() != null)
            {
                return "System.TimeSpan"; //long
            }

            if (context.TIME_OF_DAY() != null)
            {
                return "System.TimeSpan";
            }

            if (context.DATE_AND_TIME() != null)
            {
                return "System.DateTime";
            }

            if (context.STRING() != null)
            {
                return "byte[]";
            }

            if (context.BOOL() != null)
            {
                return "bool";
            }

            if (context.BYTE() != null)
            {
                return "byte";
            }

            if (context.WORD() != null)
            {
                return "ushort";
            }

            if (context.DWORD() != null)
            {
                return "uint";
            }

            if (context.LWORD() != null)
            {
                return "ulong";
            }

            if (context.UDINT() != null)
            {
                return "uint";
            }

            if (context.LTIME() != null)
            {
                return "System.DateTime";
            }

            if (context.LDATE() != null)
            {
                return "System.DateTime";
            }

            if (context.WSTRING() != null)
            {
                return "char[]";
            }

            if (context.CHAR() != null)
            {
                return "byte";
            }

            if (context.WCHAR() != null)
            {
                return "char";
            }

            if (context.ID() != null)
            {
                return context.ID().ToString();
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertAssignValueToString(STParser.AssignValueContext context)
        {
            if (context.bracketedAssignOperation() != null)
            {
                return ConvertBracketedAssignOperationToString(context.bracketedAssignOperation());
            }

            if (context.assignOperation() != null)
            {
                return ConvertAssignOperationToString(context.assignOperation());
            }

            if (context.bracketedValue() != null)
            {
                return ConvertBracketedValueToString(context.bracketedValue());
            }

            if (context.value() != null)
            {
                return ConvertValueToString(context.value());
            }

            if (context.variableChoice() != null)
            {
                return ConvertVariableChoiceToString(context.variableChoice());
            }

            if (context.structAssign() != null)
            {
                return ConvertStructAssignToString(context.structAssign());
            }
            //if (context.referenceAssignment() != null) return ConvertReferenceAssignmentToString(context.referenceAssignment());
            if (context.classPropertyAccess() != null)
            {
                return ConvertClassPropertyAccessToString(context.classPropertyAccess());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertBracketedAssignOperationToString(STParser.BracketedAssignOperationContext context)
        {
            return $"( {ConvertAssignOperationToString(context.assignOperation())} )";
        }

        private string ConvertAssignOperationToString(STParser.AssignOperationContext context)
        {
            if (context.bracketedArithmeticOperation() != null)
            {
                return ConvertBracketedArithmeticOperationToString(context.bracketedArithmeticOperation());
            }

            if (context.arithmeticOperation() != null)
            {
                return ConvertArithmeticOperationToString(context.arithmeticOperation());
            }

            if (context.bracketedNegatedOperation() != null)
            {
                return ConvertBracketedNegatedOperationToString(context.bracketedNegatedOperation());
            }

            if (context.negatedOperation() != null)
            {
                return ConvertNegatedOperationToString(context.negatedOperation());
            }

            if (context.bracketedLogicalRelationalOperation() != null)
            {
                return ConvertBracketedLogicalRelationalOperationToString(context.bracketedLogicalRelationalOperation());
            }

            if (context.logicalRelationalOperation() != null)
            {
                return ConvertLogicalRelationalOperationToString(context.logicalRelationalOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertOperationToString(STParser.OperationContext context)
        {
            if (context.bracketedArithmeticOperation() != null)
            {
                return ConvertBracketedArithmeticOperationToString(context.bracketedArithmeticOperation());
            }

            if (context.arithmeticOperation() != null)
            {
                return ConvertArithmeticOperationToString(context.arithmeticOperation());
            }

            if (context.bracketedNegatedOperation() != null)
            {
                return ConvertBracketedNegatedOperationToString(context.bracketedNegatedOperation());
            }

            if (context.negatedOperation() != null)
            {
                return ConvertNegatedOperationToString(context.negatedOperation());
            }

            if (context.bracketedLogicalRelationalOperation() != null)
            {
                return ConvertBracketedLogicalRelationalOperationToString(context.bracketedLogicalRelationalOperation());
            }

            if (context.logicalRelationalOperation() != null)
            {
                return ConvertLogicalRelationalOperationToString(context.logicalRelationalOperation());
            }

            if (context.trueOrFalseOperation() != null)
            {
                return ConvertTrueOrFalseOperationToString(context.trueOrFalseOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertBracketedOperationToString(STParser.BracketedOperationContext context)
        {
            return $"({ConvertOperationToString(context.operation())})";
        }

        private string ConvertBracketedArithmeticOperationToString(STParser.BracketedArithmeticOperationContext context)
        {
            string body = "(";
            string previousOperationBody = "";
            int operatorIndex = 0, valueIndex = 0;

            if (context.arithmeticOperator()[0]?.EXPONENT_OPERATOR() != null)
            {
                body += previousOperationBody = $"System.Math.Pow({ConvertValueToString(context.value())}, " +
                    $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])})";

                operatorIndex++;
                valueIndex++;
            }
            else
            {
                body += ConvertValueToString(context.value());
            }

            while (operatorIndex < context.arithmeticOperator().Length)
            {
                string parsedOperator;

                if (context.arithmeticOperator()[operatorIndex]?.EXPONENT_OPERATOR() != null)
                {
                    body = previousOperationBody = $"System.Math.Pow({previousOperationBody}, " +
                        $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])})";

                    operatorIndex++;
                    valueIndex++;
                    continue;
                }
                else if (operatorIndex + 1 < context.arithmeticOperator().Length && context.arithmeticOperator()[operatorIndex + 1]?.EXPONENT_OPERATOR() != null)
                {
                    parsedOperator = ConvertArithmeticOperatorToString(context.arithmeticOperator()[operatorIndex]);
                    if (string.IsNullOrEmpty(parsedOperator))
                        return null;

                    body += parsedOperator;
                    body += $"System.Math.Pow({ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])}, " +
                        $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex + 1])})";

                    operatorIndex = operatorIndex + 2;
                    valueIndex = valueIndex + 2;
                    continue;
                }

                parsedOperator = ConvertArithmeticOperatorToString(context.arithmeticOperator()[operatorIndex]);
                if (string.IsNullOrEmpty(parsedOperator))
                    return null;

                body += parsedOperator;
                body += ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex]);

                operatorIndex++;
                valueIndex++;
            }

            body += ")";

            return body;
        }

        private string ConvertArithmeticOperationToString(STParser.ArithmeticOperationContext context)
        {
            string body = "";
            string previousOperationBody = "";
            int operatorIndex = 0, valueIndex = 0;

            if (context.arithmeticOperator()[0]?.EXPONENT_OPERATOR() != null)
            {
                body += previousOperationBody = $"System.Math.Pow({ConvertValueToString(context.value())}, " +
                    $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])})";

                operatorIndex++;
                valueIndex++;
            }
            else
            {
                body += ConvertValueToString(context.value());
            }

            while (operatorIndex < context.arithmeticOperator().Length)
            {
                string parsedOperator;

                if (context.arithmeticOperator()[operatorIndex]?.EXPONENT_OPERATOR() != null)
                {
                    body = previousOperationBody = $"System.Math.Pow({previousOperationBody}, " +
                        $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])})";

                    operatorIndex++;
                    valueIndex++;
                    continue;
                }
                else if (operatorIndex + 1 < context.arithmeticOperator().Length && context.arithmeticOperator()[operatorIndex + 1]?.EXPONENT_OPERATOR() != null)
                {
                    body += parsedOperator = ConvertArithmeticOperatorToString(context.arithmeticOperator()[operatorIndex]);
                    if (string.IsNullOrEmpty(parsedOperator))
                        return null;

                    body += parsedOperator;
                    body += $"System.Math.Pow({ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex])}, " +
                        $"{ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex + 1])})";

                    operatorIndex = operatorIndex + 2;
                    valueIndex = valueIndex + 2;
                    continue;
                }

                parsedOperator = ConvertArithmeticOperatorToString(context.arithmeticOperator()[operatorIndex]);
                if (string.IsNullOrEmpty(parsedOperator))
                    return null;

                body += parsedOperator;
                body += ConvertArithmeticOperationValueToString(context.arithmeticOperationValue()[valueIndex]);

                operatorIndex++;
                valueIndex++;
            }

            return body;
        }

        private string ConvertArithmeticOperationValueToString(STParser.ArithmeticOperationValueContext context)
        {
            if (context.ID() != null)
            {
                string name = context.ID().ToString();
                if (globalVariablesNames.Contains(name))
                {
                    return "GLOBAL_VARIABLES." + name;
                }
                else
                {
                    return name;
                }
            }

            if (context.value() != null)
            {
                return ConvertValueToString(context.value());
            }

            if (context.bracketedArithmeticOperation() != null)
            {
                return ConvertBracketedArithmeticOperationToString(context.bracketedArithmeticOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertBracketedNegatedOperationToString(STParser.BracketedNegatedOperationContext context)
        {
            string body = "!(";

            if (context.logicalRelationalOperation() != null)
            {
                body += ConvertLogicalRelationalOperationToString(context.logicalRelationalOperation());
            }

            if (context.bracketedNegatedOperation() != null)
            {
                body += ConvertBracketedNegatedOperationToString(context.bracketedNegatedOperation());
            }

            if (context.trueOrFalseOperation() != null)
            {
                body += ConvertTrueOrFalseOperationToString(context.trueOrFalseOperation());
            }

            body += ")";

            return body;
        }

        private string ConvertNegatedOperationToString(STParser.NegatedOperationContext context)
        {
            string body = "!";

            if (context.BRACKET_OPEN() != null)
            {
                body += "(";
            }

            if (context.logicalRelationalOperation() != null)
            {
                body += ConvertLogicalRelationalOperationToString(context.logicalRelationalOperation());
            }

            if (context.trueOrFalseOperation() != null)
            {
                body += ConvertTrueOrFalseOperationToString(context.trueOrFalseOperation());
            }

            if (context.BRACKET_CLOSE() != null)
            {
                body += ")";
            }

            return body;
        }

        private string ConvertBracketedLogicalRelationalOperationToString(STParser.BracketedLogicalRelationalOperationContext context)
        {
            string body = $"( {ConvertLogicalRelationalValueToString(context.logicalRelationalValue())}";

            foreach (var logicalOperation in context.logicalOperation())
            {
                body += ConvertLogicalOperationToString(logicalOperation);
            }

            body += ")";

            foreach (var logicalOperationOutside in context.logicalOperationOutside())
            {
                body += ConvertLogicalOperationToString(logicalOperationOutside.logicalOperation());
            }

            return body;
        }

        private string ConvertLogicalRelationalOperationToString(STParser.LogicalRelationalOperationContext context)
        {
            string body = ConvertLogicalRelationalValueToString(context.logicalRelationalValue());

            foreach (var logicalOperation in context.logicalOperation())
            {
                body += ConvertLogicalOperationToString(logicalOperation);
            }

            return body;
        }

        private string ConvertTrueOrFalseOperationToString(STParser.TrueOrFalseOperationContext context)
        {
            if (context.ID() != null)
            {
                return context.ID().ToString();
            }

            if (context.TRUE() != null)
            {
                return context.TRUE().ToString();
            }

            if (context.FALSE() != null)
            {
                return context.FALSE().ToString();
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertArithmeticOperatorToString(STParser.ArithmeticOperatorContext context)
        {
            if (context.ADD_OPERATOR() != null)
            {
                return "+";
            }

            if (context.SUBTRACT_OPERATOR() != null)
            {
                return "-";
            }

            if (context.MULTIPLY_OPERATOR() != null)
            {
                return "*";
            }

            if (context.DIVISION_OPERATOR() != null)
            {
                return "/";
            }

            if (context.MODULO_OPERATOR() != null)
            {
                return "%";
            }

            return null;
        }

        private string ConvertLogicalOperationToString(STParser.LogicalOperationContext context)
        {
            string body = $"{ConvertLogicalRelationalOperatorToString(context.logicalRelationalOperator())} {ConvertLogicalRelationalValueToString(context.logicalRelationalValue())}";

            return body;
        }

        private string ConvertLogicalRelationalOperatorToString(STParser.LogicalRelationalOperatorContext context)
        {
            if (context.logicalOperator() != null)
            {
                return ConvertLogicalOperatorToString(context.logicalOperator());
            }

            if (context.relationalOperator() != null)
            {
                return ConvertRelationalOperatorToString(context.relationalOperator());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertLogicalRelationalValueToString(STParser.LogicalRelationalValueContext context)
        {
            if (context.arithmeticOperation() != null)
            {
                return ConvertArithmeticOperationToString(context.arithmeticOperation());
            }

            if (context.value() != null)
            {
                return ConvertValueToString(context.value());
            }

            if (context.bracketedLogicalRelationalOperation() != null)
            {
                return ConvertBracketedLogicalRelationalOperationToString(context.bracketedLogicalRelationalOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertLogicalOperatorToString(STParser.LogicalOperatorContext context)
        {
            if (context.AND_OPERATOR() != null)
            {
                return "&&";
            }

            if (context.OR_OPERATOR() != null)
            {
                return "||";
            }

            if (context.XOR_OPERATOR() != null)
            {
                return "^";
            }

            if (context.NEGATION_OPERATOR() != null)
            {
                return "!";
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertRelationalOperatorToString(STParser.RelationalOperatorContext context)
        {
            if (context.EQUAL_OPERATOR() != null)
            {
                return "==";
            }

            if (context.LESS_THAN_OPERATOR() != null)
            {
                return "<";
            }

            if (context.LESS_THAN_EQUAL_OPERATOR() != null)
            {
                return "<=";
            }

            if (context.GREATER_THAN_OPERATOR() != null)
            {
                return ">";
            }

            if (context.GREATER_THAN_EQUAL_OPERATOR() != null)
            {
                return ">=";
            }

            if (context.NOT_EQUAL_OPERATOR() != null)
            {
                return "!=";
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertBracketedValueToString(STParser.BracketedValueContext context)
        {
            return $"({ConvertValueToString(context.value())})";
        }

        private string ConvertValueToString(STParser.ValueContext context)
        {
            if (context.TRUE() != null)
            {
                return "true";
            }

            if (context.FALSE() != null)
            {
                return "false";
            }

            if (context.STRING_VALUE() != null)
            {
                return context.STRING_VALUE().ToString().Replace("'", "\"");
            }

            if (context.NUMBER_VALUE() != null)
            {
                return context.NUMBER_VALUE().ToString();
            }

            if (context.THIS() != null)
            {
                return "this";
            }

            if (context.ID() != null)
            {
                string value = context.ID().ToString();

                //if (context.partialBitAccess() != null)
                //    value += ConvertPartialBitAccess(context.partialBitAccess());

                if (globalVariablesNames.Contains(value))
                {
                    return "GLOBAL_VARIABLES." + value;
                }
                else
                {
                    return value;
                }
            }
            if (context.ADDRESS() != null)
            {
                return "";
            }

            if (context.functionCall() != null)
            {
                return ConvertFunctionCallToString(context.functionCall());
            }

            if (context.classMethodCall() != null)
            {
                return ConvertClassMethodCallToString(context.classMethodCall());
            }

            if (context.parentClassMethodCall() != null)
            {
                return ConvertParentClassMethodCallToString(context.parentClassMethodCall());
            }

            if (context.anonymousFunction() != null)
            {
                return "";
            }

            if (context.THIS() != null)
            {
                return "this";
            }

            if (context.idWithLength() != null)
            {
                return ConvertIdWithLengthToString(context.idWithLength());
            }

            if (context.referenceValue() != null)
            {
                return "";
            }

            if (context.valueWithQuantity() != null)
            {
                return ConvertValueWithQuantityToString(context.valueWithQuantity());
            }

            if (context.arrayValue() != null)
            {
                return ConvertArrayValueToString(context.arrayValue());
            }

            if (context.classObjectMethodCall() != null)
            {
                return ConvertClassObjectMethodCallToString(context.classObjectMethodCall());
            }

            if (context.namespaceAccess() != null)
            {
                return ConvertNamespaceAccessToString(context.namespaceAccess());
            }

            if (context.referenceAccess() != null)
            {
                return "";
            }

            if (context.registryAccess() != null)
            {
                return "";
            }

            if (context.trueOrFalseOperation() != null)
            {
                return ConvertTrueOrFalseOperationToString(context.trueOrFalseOperation());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertNumberValueToString(STParser.NumberValueContext context)
        {
            if (context.NUMBER_VALUE() != null)
            {
                return context.NUMBER_VALUE().ToString();
            }

            if (context.ID() != null)
            {
                return context.ID().ToString();
            }

            if (context.functionCall() != null)
            {
                return ConvertFunctionCallToString(context.functionCall());
            }

            if (context.classMethodCall() != null)
            {
                return ConvertClassMethodCallToString(context.classMethodCall());
            }

            if (context.parentClassMethodCall() != null)
            {
                return ConvertParentClassMethodCallToString(context.parentClassMethodCall());
            }

            return $"\n/*\n\tCannot convert:\n\t\t {context.GetText()} \n*/\n\n";
        }

        private string ConvertGenericDataTypeToString(STParser.GenericDataTypeContext context)
        {
            //if (context.ANY() != null) return "";
            //if (context.ANY_DERIVED() != null) return "";
            //if (context.ANY_ELEMENTARY() != null) return "";
            //if (context.ANY_MAGNITUDE() != null) return "";
            //if (context.ANY_NUM() != null) return "";
            //if (context.ANY_REAL() != null) return "";
            //if (context.ANY_INT() != null) return "";
            //if (context.ANY_UNSIGNED() != null) return "";
            //if (context.ANY_SIGNED() != null) return "";
            //if (context.ANY_DURATION() != null) return "";
            //if (context.ANY_BIT() != null) return "";
            //if (context.ANY_CHARS() != null) return "";
            //if (context.ANY_STRING() != null) return "";
            //if (context.ANY_CHAR() != null) return "";
            //if (context.ANY_DATE() != null) return "";
            //return "";
            return "var";
        }
    }
}