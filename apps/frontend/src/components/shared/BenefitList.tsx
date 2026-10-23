import type { Benefit } from '../../mocks/marketing'
import classes from './BenefitList.module.css'

type BenefitListProps = {
  items: readonly Benefit[]
  variant: 'marketing' | 'details'
}

export function BenefitList({ items, variant }: BenefitListProps) {
  return (
    <ul className={`${classes.list} ${variant === 'details' ? classes.details : classes.marketing}`} aria-label="Benefícios">
      {items.map((item) => (
        <li className={classes.item} key={item.id}>
          <span className={classes.iconTile}>
            <img src={variant === 'marketing' ? item.marketingIcon : item.detailsIcon} alt="" width="48" height="48" loading="lazy" />
          </span>
          <div>
            <h3>{item.title}</h3>
            <p>{item.description}</p>
          </div>
        </li>
      ))}
    </ul>
  )
}
