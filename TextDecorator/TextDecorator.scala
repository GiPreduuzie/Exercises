trait Entry{
  def from : Int
  def to : Int
  def acceptVisitor[T](visitor : Visitor[T]) : T
}

case class Link(from: Int, to: Int) extends Entry {
  override def acceptVisitor[T](visitor: Visitor[T]): T = visitor.visit(this)
}

case class Entity(from: Int, to: Int) extends Entry {
  override def acceptVisitor[T](visitor: Visitor[T]): T = visitor.visit(this)
}

case class TwitterUsername(from: Int, to: Int) extends Entry {
  override def acceptVisitor[T](visitor: Visitor[T]): T = visitor.visit(this)
}

trait Visitor[T]{
  def visit(entry: Link) : T
  def visit(entry: Entity) : T
  def visit(entry: TwitterUsername) : T
}

class TextDecorator {
  def DecorateText(originalValue: String, entries: Array[Entry]): String = {
    val decorator = new EntryDecorator
    var currentIndex = 0
    var result = ""

    for (entry <- entries.sortBy(x => x.from)) {
      result += originalValue.substring(currentIndex, entry.from)
      result += entry.acceptVisitor(decorator)(originalValue.substring(entry.from, entry.to))
      currentIndex = entry.to
    }

    result + originalValue.substring(currentIndex, originalValue.length)
  }
}

class EntryDecorator extends Visitor[String => String] {
  override def visit(entry: Link): String => String = value => "<a href='%s'>%s</a>".format(value, value)

  override def visit(entry: Entity): String => String = value => "<bold>%s</bold>".format(value)

  override def visit(entry: TwitterUsername): String => String = value => {
    val username = value.substring(1)
    "@<a href='http://twitter.com/%s'>%s</a>".format(username, username)
  }
}

