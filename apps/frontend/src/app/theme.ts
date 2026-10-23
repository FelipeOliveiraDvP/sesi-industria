import { createTheme, type MantineColorsTuple } from '@mantine/core'

const sesiRed: MantineColorsTuple = [
  '#fce9e9',
  '#f8d1d2',
  '#f3b7b9',
  '#ed9b9f',
  '#e77d82',
  '#d6535b',
  '#c3222c',
  '#aa111d',
  '#890000',
  '#690009',
]

const peach: MantineColorsTuple = [
  '#fff9f0',
  '#fff1df',
  '#ffe7c9',
  '#ffddb2',
  '#fcd19c',
  '#f5c58c',
  '#e7b77e',
  '#d6a66c',
  '#c4955b',
  '#ab7b44',
]

export const theme = createTheme({
  primaryColor: 'sesiRed',
  colors: { sesiRed, peach },
  fontFamily: '"Open Sans", Arial, sans-serif',
  headings: {
    fontFamily: '"Open Sans", Arial, sans-serif',
    fontWeight: '700',
  },
  fontSizes: {
    xs: '0.75rem',
    sm: '0.875rem',
    md: '1rem',
    lg: '1.125rem',
    xl: '1.25rem',
  },
  spacing: {
    xs: '0.5rem',
    sm: '0.75rem',
    md: '1rem',
    lg: '1.5rem',
    xl: '2rem',
  },
  radius: {
    xs: '0.25rem',
    sm: '0.5rem',
    md: '0.625rem',
    lg: '0.875rem',
    xl: '1.25rem',
  },
  breakpoints: {
    xs: '36em',
    sm: '48em',
    md: '62em',
    lg: '75em',
    xl: '88em',
  },
})
