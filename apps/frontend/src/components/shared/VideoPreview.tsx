import { assetPaths } from '../../app/assets'
import classes from './VideoPreview.module.css'

type VideoPreviewProps = {
  description: string
  imageUrl?: string
  imageAlt?: string
}

export function VideoPreview({
  description,
  imageUrl = assetPaths.images.videoThumbnail,
  imageAlt = 'Dois profissionais em um ambiente industrial demonstrando um equipamento.',
}: VideoPreviewProps) {
  return (
    <figure className={classes.preview}>
      <img
        className={classes.photo}
        src={imageUrl}
        alt={imageAlt}
        loading="lazy"
        decoding="async"
      />
      <span className={classes.overlay} aria-hidden="true" />
      <span className={classes.play} aria-hidden="true">
        <span />
      </span>
      <figcaption className={classes.visuallyHidden}>{description}</figcaption>
    </figure>
  )
}
