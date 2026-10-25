import { Component, type ReactNode } from 'react'
import classes from './ErrorBoundary.module.css'

type ErrorBoundaryProps = {
  children: ReactNode
  contextKey: string
  title: string
  description: string
  returnHref: string
  returnLabel: string
  onRetry?: () => void | Promise<unknown>
  retryLabel?: string
}

type ErrorBoundaryState = {
  hasError: boolean
}

export class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
  state: ErrorBoundaryState = { hasError: false }

  static getDerivedStateFromError(): ErrorBoundaryState {
    return { hasError: true }
  }

  componentDidUpdate(previousProps: ErrorBoundaryProps) {
    if (this.state.hasError && previousProps.contextKey !== this.props.contextKey) {
      this.setState({ hasError: false })
    }
  }

  private retry = async () => {
    try {
      await this.props.onRetry?.()
    } catch {
      // The query layer owns communication failures; the boundary only resets rendering.
    }
    this.setState({ hasError: false })
  }

  render() {
    if (!this.state.hasError) return this.props.children

    return (
      <main className={classes.state} tabIndex={-1}>
        <section role="alert" aria-labelledby="render-error-title">
          <h1 id="render-error-title">{this.props.title}</h1>
          <p>{this.props.description}</p>
          <div className={classes.actions}>
            {this.props.onRetry && (
              <button className={classes.retry} type="button" onClick={() => void this.retry()}>
                {this.props.retryLabel ?? 'Tentar novamente'}
              </button>
            )}
            <a className={classes.returnLink} href={this.props.returnHref}>{this.props.returnLabel}</a>
          </div>
        </section>
      </main>
    )
  }
}
