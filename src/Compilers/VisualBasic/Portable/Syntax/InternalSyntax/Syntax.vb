﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Generic
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.Syntax.InternalSyntax
    Partial Friend Class SyntaxFactory
        Friend Shared ReadOnly CarriageReturnLineFeed As SyntaxTrivia = EndOfLine(vbCrLf)
        Friend Shared ReadOnly LineFeed As SyntaxTrivia = EndOfLine(vbLf)
        Friend Shared ReadOnly CarriageReturn As SyntaxTrivia = EndOfLine(vbCr)
        Friend Shared ReadOnly Space As SyntaxTrivia = Whitespace(" ")
        Friend Shared ReadOnly Tab As SyntaxTrivia = Whitespace(vbTab)

        Friend Shared ReadOnly ElasticCarriageReturnLineFeed As SyntaxTrivia = EndOfLine(vbCrLf, elastic:=True)
        Friend Shared ReadOnly ElasticLineFeed As SyntaxTrivia = EndOfLine(vbLf, elastic:=True)
        Friend Shared ReadOnly ElasticCarriageReturn As SyntaxTrivia = EndOfLine(vbCr, elastic:=True)
        Friend Shared ReadOnly ElasticSpace As SyntaxTrivia = Whitespace(" ", elastic:=True)
        Friend Shared ReadOnly ElasticTab As SyntaxTrivia = Whitespace(vbTab, elastic:=True)

        Friend Shared ReadOnly ElasticZeroSpace As SyntaxTrivia = Whitespace(String.Empty, elastic:=True)

        Friend Shared Function EndOfLine(text As String, Optional elastic As Boolean = False) As SyntaxTrivia

            Dim trivia As SyntaxTrivia = Nothing

            ' use predefined trivia
            Select Case text
                Case vbCr
                    trivia = If(elastic, SyntaxFactory.ElasticCarriageReturn, SyntaxFactory.CarriageReturn)
                Case vbLf
                    trivia = If(elastic, SyntaxFactory.ElasticLineFeed, SyntaxFactory.LineFeed)
                Case vbCrLf
                    trivia = If(elastic, SyntaxFactory.ElasticCarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed)
            End Select

            ' note: predefined trivia might not yet be defined during initialization
            If trivia IsNot Nothing Then
                Return trivia
            End If

            trivia = SyntaxTrivia(SyntaxKind.EndOfLineTrivia, text)
            If Not elastic Then
                Return trivia
            End If

            Return trivia.WithAnnotations(SyntaxAnnotation.ElasticAnnotation)
        End Function

        Friend Shared Function Whitespace(text As String, Optional elastic As Boolean = False) As SyntaxTrivia
            Dim trivia = SyntaxTrivia(SyntaxKind.WhitespaceTrivia, text)
            If Not elastic Then
                Return trivia
            End If

            Return trivia.WithAnnotations(SyntaxAnnotation.ElasticAnnotation)
        End Function

        Friend Shared Function Token(leading As VisualBasicSyntaxNode, kind As SyntaxKind, trailing As VisualBasicSyntaxNode, Optional text As String = Nothing) As SyntaxToken
            Return SyntaxToken.Create(kind, leading, trailing, If(text Is Nothing, SyntaxFacts.GetText(kind), text))
        End Function

        Friend Shared Function GetWellKnownTrivia() As IEnumerable(Of SyntaxTrivia)
            Return New SyntaxTrivia() {
                CarriageReturn,
                CarriageReturnLineFeed,
                LineFeed,
                Space,
                Tab,
                ElasticCarriageReturn,
                ElasticLineFeed,
                ElasticCarriageReturnLineFeed,
                ElasticSpace,
                ElasticTab,
                ElasticZeroSpace,
                Whitespace("  "),
                Whitespace("   "),
                Whitespace("    ")
                }
        End Function

    End Class
End Namespace
