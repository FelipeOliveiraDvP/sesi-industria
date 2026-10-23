import { ActionIcon } from '@mantine/core'
import { useCallback, useEffect, useId, useRef, useState, type KeyboardEvent, type ReactNode } from 'react'
import { assetPaths } from '../../app/assets'
import classes from './HorizontalRail.module.css'

type HorizontalRailProps = {
  children: ReactNode
  label: string
  heading?: ReactNode
}

export function HorizontalRail({ children, label, heading }: HorizontalRailProps) {
  const viewportRef = useRef<HTMLDivElement>(null)
  const viewportId = useId()
  const [canScrollBackward, setCanScrollBackward] = useState(false)
  const [canScrollForward, setCanScrollForward] = useState(false)

  const updateBounds = useCallback(() => {
    const viewport = viewportRef.current
    if (!viewport) return

    setCanScrollBackward(viewport.scrollLeft > 1)
    setCanScrollForward(viewport.scrollLeft + viewport.clientWidth < viewport.scrollWidth - 1)
  }, [])

  useEffect(() => {
    const viewport = viewportRef.current
    if (!viewport) return

    updateBounds()
    viewport.addEventListener('scroll', updateBounds, { passive: true })
    window.addEventListener('resize', updateBounds)

    const observer = new ResizeObserver(updateBounds)
    observer.observe(viewport)
    if (viewport.firstElementChild) observer.observe(viewport.firstElementChild)

    return () => {
      viewport.removeEventListener('scroll', updateBounds)
      window.removeEventListener('resize', updateBounds)
      observer.disconnect()
    }
  }, [updateBounds])

  const scroll = useCallback((direction: 'backward' | 'forward') => {
    const viewport = viewportRef.current
    if (!viewport) return

    const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches
    viewport.scrollBy({
      left: (direction === 'forward' ? 1 : -1) * Math.max(viewport.clientWidth * 0.78, 240),
      behavior: reducedMotion ? 'auto' : 'smooth',
    })
  }, [])

  const handleKeyDown = (event: KeyboardEvent<HTMLDivElement>) => {
    if (event.key === 'ArrowLeft') {
      event.preventDefault()
      scroll('backward')
    } else if (event.key === 'ArrowRight') {
      event.preventDefault()
      scroll('forward')
    }
  }

  return (
    <div className={classes.root}>
      <div className={classes.header}>
        <div className={classes.heading}>{heading}</div>
        <div className={classes.controls}>
          <ActionIcon
            type="button"
            variant="outline"
            size="lg"
            aria-label={`Voltar no trilho: ${label}`}
            aria-controls={viewportId}
            disabled={!canScrollBackward}
            onClick={() => scroll('backward')}
          >
            <img src={assetPaths.controls.previous} alt="" width="24" height="24" />
          </ActionIcon>
          <ActionIcon
            type="button"
            variant="outline"
            size="lg"
            aria-label={`Avançar no trilho: ${label}`}
            aria-controls={viewportId}
            disabled={!canScrollForward}
            onClick={() => scroll('forward')}
          >
            <img src={assetPaths.controls.next} alt="" width="24" height="24" />
          </ActionIcon>
        </div>
      </div>

      <div
        id={viewportId}
        ref={viewportRef}
        className={classes.viewport}
        role="region"
        aria-label={label}
        aria-roledescription="carrossel"
        tabIndex={0}
        onKeyDown={handleKeyDown}
      >
        {children}
      </div>
    </div>
  )
}
