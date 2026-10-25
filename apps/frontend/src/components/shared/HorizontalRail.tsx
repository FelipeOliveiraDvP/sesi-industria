import { ActionIcon } from '@mantine/core'
import {
  useCallback,
  useEffect,
  useId,
  useRef,
  useState,
  type KeyboardEvent,
  type MouseEvent,
  type PointerEvent,
  type ReactNode,
} from 'react'
import { assetPaths } from '../../app/assets'
import classes from './HorizontalRail.module.css'

type HorizontalRailProps = {
  children: ReactNode
  label: string
  heading?: ReactNode
  footer?: ReactNode
  controlsTone?: 'dark' | 'light'
}

export function HorizontalRail({ children, label, heading, footer, controlsTone = 'dark' }: HorizontalRailProps) {
  const viewportRef = useRef<HTMLDivElement>(null)
  const viewportId = useId()
  const dragRef = useRef<{ pointerId: number; startX: number; startScrollLeft: number; moved: boolean } | null>(null)
  const suppressClickRef = useRef(false)
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

  const handlePointerDown = (event: PointerEvent<HTMLDivElement>) => {
    const viewport = viewportRef.current
    if (
      !viewport
      || event.pointerType === 'touch'
      || event.button !== 0
      || !event.isPrimary
      || viewport.scrollWidth <= viewport.clientWidth
    ) return

    const target = event.target
    if (!(target instanceof Element) || target.closest('button, input, select, textarea, [contenteditable="true"]')) return

    dragRef.current = {
      pointerId: event.pointerId,
      startX: event.clientX,
      startScrollLeft: viewport.scrollLeft,
      moved: false,
    }
    target.setPointerCapture(event.pointerId)
  }

  const handlePointerMove = (event: PointerEvent<HTMLDivElement>) => {
    const viewport = viewportRef.current
    const drag = dragRef.current
    if (!viewport || !drag || drag.pointerId !== event.pointerId) return

    const distance = event.clientX - drag.startX
    if (!drag.moved && Math.abs(distance) > 5) {
      drag.moved = true
      viewport.dataset.dragging = 'true'
      window.getSelection()?.removeAllRanges()
    }

    if (!drag.moved) return
    if (event.cancelable) event.preventDefault()
    viewport.scrollLeft = drag.startScrollLeft - distance
  }

  const finishPointerDrag = (event: PointerEvent<HTMLDivElement>, suppressClick: boolean) => {
    const drag = dragRef.current
    if (!drag || drag.pointerId !== event.pointerId) return

    dragRef.current = null
    viewportRef.current?.removeAttribute('data-dragging')

    if (suppressClick && drag.moved) {
      suppressClickRef.current = true
      window.setTimeout(() => {
        suppressClickRef.current = false
      }, 0)
    }
  }

  const handleClickCapture = (event: MouseEvent<HTMLDivElement>) => {
    if (!suppressClickRef.current || event.detail === 0) return

    event.preventDefault()
    event.stopPropagation()
    suppressClickRef.current = false
  }

  return (
    <div className={classes.root}>
      {heading && (
        <div className={classes.header}>
          <div className={classes.heading}>{heading}</div>
        </div>
      )}

      <div
        id={viewportId}
        ref={viewportRef}
        className={classes.viewport}
        role="region"
        aria-label={label}
        aria-roledescription="carrossel"
        tabIndex={0}
        onKeyDown={handleKeyDown}
        onPointerDown={handlePointerDown}
        onPointerMove={handlePointerMove}
        onPointerUp={(event) => finishPointerDrag(event, true)}
        onPointerCancel={(event) => finishPointerDrag(event, false)}
        onLostPointerCapture={(event) => finishPointerDrag(event, false)}
        onClickCapture={handleClickCapture}
      >
        {children}
      </div>

      <div className={classes.footer}>
        {footer && <div className={classes.footerContent}>{footer}</div>}
        <div className={`${classes.controls}${controlsTone === 'light' ? ` ${classes.lightControls}` : ''}`}>
          <ActionIcon
            type="button"
            variant="transparent"
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
            variant="transparent"
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
    </div>
  )
}
